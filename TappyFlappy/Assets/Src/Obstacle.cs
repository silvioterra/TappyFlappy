﻿using UnityEngine;
using System.Collections;
using System;

public class Obstacle : MonoBehaviour, IScreenListener, ISpeedListener
{

    public const float MaxHeightScreenPercent = 0.5f;

    protected bool mInUse;
    protected Vector2 mPositionInWorld;
    protected float mWorldHeight;
    protected float mWidth = 2.0f;

    public float Width
    {
        get { return mWidth; }
        set { mWidth = value; }
    }

    public float WorldPosition
    {
        get { return mPositionInWorld.x; }
        set { SetWorldPosition(value); }
    }

    public bool Used
    {
        get
        {
            return mInUse;
        }

        set
        {
            SetUsed(value);
        }
    }

    void IScreenListener.OnScreenResolutionChanged(int width, int height)
    {
        UpdateMaxHeight();
    }

    virtual protected void UpdateMaxHeight()
    {
        Vector3 top = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0));
        Vector3 bottom = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 0));
        mWorldHeight = (bottom - top).y;

        SetWorldPosition(mPositionInWorld.x);
    }


    public void Hide()
    {
        // Just moving it out of the way for now...
        this.transform.position = new Vector3(-20, -20, 0);
        //enabled = false;
    }

    virtual protected void InternalAwake()
    {
        mPositionInWorld = Vector2.zero;
    }
    void Awake()
    {
        InternalAwake();
    }

    // Use this for initialization
    void Start()
    {
        Game.GetInstance().AddScreenListener(this);
        Game.GetInstance().GetLevelManager().AddSpeedListener(this);
        UpdateMaxHeight();
    }

    // Update is called once per frame
    void Update()
    {

    }


    public virtual void SetUsed(bool used)
    {
        if (mInUse == used)
        {
            return;
        }
        mInUse = used;
        //enabled = used;
        if (!used)
        {
            Hide();
        }
        else
        {
            UpdateMaxHeight();
        }
    }

    virtual protected void SetWorldPosition(float xPosition)
    {
        // This really probably shouldn't change once set
        mPositionInWorld = new Vector2(xPosition, 1.0f);
    }

    void ISpeedListener.OnScreenCenterPositionUpdated(float worldPosition)
    {
        if (mInUse)
        {
            // Determine the new screen position for this turret

            // This is how far we are from the center of the screen, in world units
            float xPos = mPositionInWorld.x - worldPosition;
            Vector3 placement = Camera.main.ViewportToWorldPoint(new Vector3(xPos, 0, 0));
            PlaceOnWorld(xPos, -Camera.main.orthographicSize + mPositionInWorld.y / 2.0f, 0);
            //-Camera.main.orthographicSize + (newObstacle.mHeight)
        }

    }

    virtual protected void PlaceOnWorld(float x, float y, int z)
    {
        this.transform.position = new Vector3(x, y, z);
    }

    void ISpeedListener.OnSpeedChanged(float newSpeed)
    {
        //throw new NotImplementedException();
    }
}
