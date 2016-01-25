using UnityEngine;
using System.Collections;
using System;

public class Turret : MonoBehaviour, IScreenListener, ISpeedListener
{

    public const float MaxHeightScreenPercent = 0.33f;

    float mMaxHeight;
    GameObject mShaft;
    private BoxCollider2D mShaftCollider;
    private float mTurretHeight = 1.0f;
    private float mTurretWidth = 2.0f;
    private bool mInUse;
    private Vector2 mPositionInWorld;
    private float mWorldHeight;

    public float Width
    {
        get { return mTurretWidth; }
        set { mTurretWidth = value; }
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

    private void UpdateMaxHeight()
    {
        Vector3 top = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0));
        Vector3 bottom = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 0));
        mWorldHeight = (bottom - top).y;

        mMaxHeight = MaxHeightScreenPercent;// Camera.main.orthographicSize * 2.0f;
        SetHeight(mTurretHeight);
        SetWorldPosition(mPositionInWorld.x);

    }

    public void SetHeight(float height)
    {
        mTurretHeight = Math.Min(height, mMaxHeight) * mWorldHeight;

        // Adjust Shaft scale
        mShaft.transform.localScale = new Vector3(mTurretWidth, mTurretHeight, 0.0f);
        // Update the tiling for the turret
        mShaft.GetComponent<Renderer>().material.mainTextureScale = new Vector2(1.0f, mTurretHeight / 2.0f);
        // TODO - remove the actual gun height from this height. 

        // Adjust the box collider and the visible elements
        mShaftCollider.size = new Vector2(mTurretWidth, mTurretHeight);

    }

    public void Hide()
    {
        // Just moving it out of the way for now...
        this.transform.position = new Vector3(-20, -20, 0);
        //enabled = false;
    }

    void Awake()
    {
        mShaft = this.transform.Find("Shaft").gameObject;
        mShaftCollider = GetComponent<BoxCollider2D>();
        mPositionInWorld = Vector2.zero;
    }

    // Use this for initialization
    void Start ()
    {
        Game.GetInstance().AddScreenListener(this);
        Game.GetInstance().GetLevelManager().AddSpeedListener(this);
        UpdateMaxHeight();
    }

    // Update is called once per frame
    void Update ()
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

    private void SetWorldPosition(float xPosition)
    {
        // This really probably shouldn't change once set
        mPositionInWorld = new Vector2(xPosition, mTurretHeight);
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
