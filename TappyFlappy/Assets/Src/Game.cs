using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Src;
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
public class Game : MonoBehaviour
{
    protected static Game sGame;
    public static Game GetInstance() { return sGame; }

    public InputManager GetInputManager() { return mInputManager; }

    List<IScreenListener> mScreenListeners;

    int mCachedScreenWidth;
    int mCachedScreenHeight;


    InputManager mInputManager;

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

        mScreenListeners = new List<IScreenListener>();
        mCachedScreenWidth = Screen.width;
        mCachedScreenHeight = Screen.height;

        // mCachedResolution = Screen.currentResolution;
    }

    // Use this for initialization
    void Start()
    {
        Cursor.visible = false;

        // keep this object around between level transitions
        UnityEngine.Object.DontDestroyOnLoad(gameObject);
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
    }

    public void AddScreenListener(IScreenListener newListener)
    {
        // Won't add it twice.
        if (!mScreenListeners.Contains(newListener))
        {
            mScreenListeners.Add(newListener);
        }
    }

    public void RemoveScreenListener(IScreenListener listener)
    {
        mScreenListeners.Remove(listener);
    }

    public void ShowLoseScreen()
    {
        // TODO
    }
}

