using UnityEngine;
using System.Collections;
using System;

public class Obstacle : MonoBehaviour, IScreenListener, ISpeedListener
{

    public const float MaxHeightScreenPercent = 0.33f;

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


    public void SetUsed(bool used)
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

            Vector3 left = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0));
            Vector3 right = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0));
            Vector3 worldWidth = right - left;

            // This is how far we are from the center of the screen, in world units
            float xPos = mPositionInWorld.x - worldPosition;
            Vector3 placement = Camera.main.ViewportToWorldPoint(new Vector3(xPos, 0, 0));
            // Hack. Without this the 33% starts from the BOTTOM of the screen. The Ground is at 1.0f
            // So this allows for a small amount of turret penetration
            const float kGroundHeight = 0.9f;
            this.transform.position = new Vector3(xPos, -Camera.main.orthographicSize + mPositionInWorld.y / 2.0f + kGroundHeight, 0);
            //-Camera.main.orthographicSize + (newObstacle.mHeight)
        }

    }

    void ISpeedListener.OnSpeedChanged(float newSpeed)
    {
        //throw new NotImplementedException();
    }
}
