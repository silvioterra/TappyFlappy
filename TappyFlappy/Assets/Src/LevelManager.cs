using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelManager : IWorldStateListener
{
    const int kMaxTurrets = 10;
    // Used to instantiate things.
    public Turret TurretPrefab;
    public float StartingWorldSpeed = 1.0f;


    // Very rough Object Pool to avoid constant instantiation/memory allocation
    List<Turret> mCachedTurrets;
    // These really should be in the LevelManager
    private float mWorldPosition;
    private float mWorldSpeed;


    List<ISpeedListener> mSpeedListeners;
    private bool mPaused;


    public LevelManager()
    {
        mSpeedListeners = new List<ISpeedListener>();
        mWorldPosition = 0.0f;
        mWorldSpeed = StartingWorldSpeed;

    }


    // Use this for initialization
    public void Start ()
    {
        mCachedTurrets = new List<Turret>();
        // Let's go ahead and spawn the maximum number of visible turrets at any point. 
        // 
        for (int i = 0; i < kMaxTurrets; i++)
        {
            Turret t = (Turret)GameObject.Instantiate(TurretPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            t.Hide();
            mCachedTurrets.Add(t);
        }
        
    }

    // Update is called once per frame
    public void Update ()
    {
        // If not paused...
        mWorldPosition += mWorldSpeed * Time.deltaTime;

        if (mWorldSpeed != 0.0f)
        {
            foreach (ISpeedListener listener in mSpeedListeners)
            {
                listener.OnWorldPositionChanged(mWorldPosition);
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
        mPaused = true;
    }

    void IWorldStateListener.OnRestartGame()
    {
        mPaused = false;
        mWorldPosition = 0.0f;
        mWorldSpeed = StartingWorldSpeed;



    }
}
