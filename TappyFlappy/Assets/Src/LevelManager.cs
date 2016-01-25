using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelObstacle
{
    public float mWorldPosition;
    public int mType;
    public float mHeight;

    public LevelObstacle(float position, int type, float height)
    {
        mWorldPosition = position;
        mType = type;
        mHeight = height;
    }
}


public class LevelManager : IWorldStateListener, IScreenListener
{
    const int kMaxTurrets = 10;
    // Used to instantiate things.
    public Turret TurretPrefab;
    public float StartingWorldSpeed = 2.0f;


    private float mObstaclePadding = 3.0f;

    // Very rough Object Pool to avoid constant instantiation/memory allocation
    List<Turret> mCachedTurrets;

    Queue<LevelObstacle> mObstacles;

    // These really should be in the LevelManager
    private float mWorldPosition;
    private float mWorldSpeed;

    List<ISpeedListener> mSpeedListeners;
    private bool mPaused;

    private int mLevelSeed;
    private float mWorldWidth;
    private float mLastObstaclePosition;
    private bool mPlaying;
    public LevelManager()
    {
        mPlaying = false;
        mSpeedListeners = new List<ISpeedListener>();
        mWorldPosition = 0.0f;
        mLastObstaclePosition = 0;
        mWorldSpeed = StartingWorldSpeed;
        mObstacles = new Queue<LevelObstacle>();
        Vector3 left = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0));
        Vector3 right = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0));
        mWorldWidth = (right - left).x;
    }


    // Use this for initialization
    public void Start ()
    {
        mCachedTurrets = new List<Turret>();
        // Let's go ahead and spawn the maximum number of visible turrets at any point. 
        // 
        for (int i = 0; i < kMaxTurrets; i++)
        {
            Turret t = (Turret)Object.Instantiate(TurretPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            t.Hide();
            mCachedTurrets.Add(t);
        }
        ResetLevel();
    }

    // Update is called once per frame
    public void Update ()
    {
        // If not paused...
        mWorldPosition += mWorldSpeed * Time.deltaTime;

        if (mWorldSpeed != 0.0f)
        {
            // Move any turrets/other world objects
            foreach (ISpeedListener listener in mSpeedListeners)
            {
                listener.OnScreenCenterPositionUpdated(mWorldPosition);
            }

            if (mPlaying)
            {

                // Remove any turrets that might be outside of our play area
                float minusHalf = mWorldPosition - (mWorldWidth / 2.0f);
                foreach (Turret tmp in mCachedTurrets)
                {
                    if (tmp.Used)
                    {
                        if (tmp.WorldPosition <= minusHalf - tmp.Width)
                        {
                            tmp.SetUsed(false);
                        }
                    }
                }

                // Add any turrets that might be about to enter play area (plus a hack amount)
                float plusHalf = mWorldPosition + (mWorldWidth / 2.0f) + 5f;
                if (mObstacles.Count > 0 && mObstacles.Peek().mWorldPosition <= plusHalf)
                {
                    LevelObstacle newObstacle = mObstacles.Dequeue();
                    foreach (Turret tmp in mCachedTurrets)
                    {
                        if (!tmp.Used)
                        {
                            tmp.SetUsed(true);
                            tmp.SetHeight(newObstacle.mHeight);
                            tmp.WorldPosition = newObstacle.mWorldPosition;
                            break;
                        }
                    }
                }
                if (mObstacles.Count < 5)
                {
                    PrepopulateObstacles(15);
                }
            }
            
        }
    }
    public void AddSpeedListener(ISpeedListener newListener)
    {
        // Won't add it twice.
        if (!mSpeedListeners.Contains(newListener))
        {
            mSpeedListeners.Add(newListener);
        }
    }

    void IWorldStateListener.OnGameLose()
    {
        mPlaying = false;
        mPaused = true;
        mWorldSpeed = 0.0f;
        foreach (ISpeedListener listener in mSpeedListeners)
        {
            listener.OnSpeedChanged(mWorldSpeed);
        }
    }

    void IWorldStateListener.OnRestartGame()
    {
        mLevelSeed = System.Environment.TickCount;
        ResetLevel();
    }


    public void ResetLevel()
    {

        mPaused = false;
        mWorldPosition = 0.0f;
        mWorldSpeed = StartingWorldSpeed;
        foreach (ISpeedListener listener in mSpeedListeners)
        {
            listener.OnSpeedChanged(mWorldSpeed);
        }

        foreach (Turret tmp in mCachedTurrets)
        {
            tmp.Used = false;
        }
        Debug.Log("LEVEL SEED: " + mLevelSeed);
        Random.seed = mLevelSeed;
    }

    private void PrepopulateObstacles(int v)
    {
        for (int i = 0; i < v; i++)
        {
            AddRandomObstacle();
        }

    }

    private void AddRandomObstacle()
    {
        mLastObstaclePosition += mObstaclePadding;
        float position = mLastObstaclePosition;
        int type = (int)(Random.value * 3.0f);
        float height = Random.Range(0.1f, Turret.MaxHeightScreenPercent);
        mObstacles.Enqueue(new LevelObstacle(position, type, height));
    }

    void IScreenListener.OnScreenResolutionChanged(int width, int height)
    {
        // We need to now adjust the horizontal edges!
        Vector3 left = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0));
        Vector3 right = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0));
        mWorldWidth = (right - left).x;

    }

    void IWorldStateListener.OnPlayStart()
    {
        mPlaying = true;
        mObstacles.Clear();
        mLastObstaclePosition = mWorldPosition + mWorldWidth / 2.0f + 3.0f;
        PrepopulateObstacles(10);

    }
}
