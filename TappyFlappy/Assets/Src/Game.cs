using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
// Always around, started only once

/// <summary>
/// This serves as a main focus for the game loop, allowing for finer control over when/how other 
/// MonoBehaviours and Prefabs get spawned/Update(). Without something explicitly calling Update() on
/// certain objects, you can have behaviors updating at times when it's not safe or when other data
/// isn't ready for it. 
/// 
/// This also serves as a way for us to reference Prefabs for spawning WITHOUT needing to hardcode the names
/// in code or rely on non-dependency-checked strings.
/// 
/// This might be outdated knowledge, but it's how Unity behaved a few years ago; order of Update() calls
/// was non-deterministic.
/// </summary>
public class Game : MonoBehaviour, IInputListener
{
    protected static Game sGame;
    public static Game GetInstance() { return sGame; }

    // Needed by Unity to instantiate Prefabs
    public Turret TurretPrefab;

    public InputManager GetInputManager() { return mInputManager; }
    public LevelManager GetLevelManager() { return mLevelManager; }
    public BulletManager GetBulletManager() { return mBulletManager; }

    List<IScreenListener> mScreenListeners;
    List<IWorldStateListener> mWorldStateListeners;
    int mCachedScreenWidth;
    int mCachedScreenHeight;


    LevelManager mLevelManager;
    InputManager mInputManager;
    BulletManager mBulletManager;

    private bool mLost;


    void Awake()
    {
        if (sGame)
        {
            // NO DUPES ALLOWED (can happen if we reload the Scene with this on the side)
            UnityEngine.Object.DestroyImmediate(gameObject);
            return;
        }
        sGame = this;
        mInputManager = new InputManager();
        mInputManager.AddInputListener("flap", this);
        mScreenListeners = new List<IScreenListener>();
        mCachedScreenWidth = Screen.width;
        mCachedScreenHeight = Screen.height;

        mWorldStateListeners = new List<IWorldStateListener>();

        mLevelManager = new LevelManager();
        mLevelManager.TurretPrefab = TurretPrefab;

        mBulletManager = new BulletManager();


        // Sometimes the listeners add themselves, sometimes we add the listeners.
        AddWorldStateListener(mLevelManager);
        AddWorldStateListener(mBulletManager);
        AddScreenListener(mBulletManager);
        mLevelManager.AddSpeedListener(mBulletManager);


        // mCachedResolution = Screen.currentResolution;
    }

    // Use this for initialization
    void Start()
    {
        Cursor.visible = false;

        // keep this object around between level transitions
        UnityEngine.Object.DontDestroyOnLoad(gameObject);
        mLevelManager.Start();
    }

    // Update is called once per frame
    void Update()
    {
        // Soooo.... unity doesn't have a default callback for when we resize a window/change screen resolutions?
        // This seems either crazy or a lack of proper research on my part.
        if (mCachedScreenWidth != Screen.width || mCachedScreenHeight != Screen.height)
        {
            mCachedScreenWidth = Screen.width;
            mCachedScreenHeight = Screen.height;

            foreach (IScreenListener listener in mScreenListeners)
            {
                listener.OnScreenResolutionChanged(mCachedScreenWidth, mCachedScreenHeight);
            }
        }
        mInputManager.Update();
        mLevelManager.Update();
        mBulletManager.Update();
    }

    public void AddScreenListener(IScreenListener newListener)
    {
        // Won't add it twice.
        if (!mScreenListeners.Contains(newListener))
        {
            mScreenListeners.Add(newListener);
        }
    }
    public void AddWorldStateListener(IWorldStateListener newListener)
    {
        // Won't add it twice.
        if (!mWorldStateListeners.Contains(newListener))
        {
            mWorldStateListeners.Add(newListener);
        }
    }

    public void RemoveScreenListener(IScreenListener listener)
    {
        mScreenListeners.Remove(listener);
    }



    public void ShowLoseScreen()
    {
        mLost = true;
        foreach (IWorldStateListener listener in mWorldStateListeners)
        {
            listener.OnGameLose();
        }

    }

    public void StartFlapping()
    {
        foreach (IWorldStateListener listener in mWorldStateListeners)
        {
            listener.OnPlayStart();
        }
    }

    public void RestartGame()
    {
        mLost = false;
        foreach (IWorldStateListener listener in mWorldStateListeners)
        {
            listener.OnRestartGame();
        }
    }

    void IInputListener.OnInputTriggered(string name)
    {
        if (mLost == true)
        {
            // Don't care. Just restart.
            RestartGame();
        }
    }
}

