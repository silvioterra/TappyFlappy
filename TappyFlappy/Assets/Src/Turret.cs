using UnityEngine;
using System.Collections;
using System;

public class Turret : MonoBehaviour, IScreenListener
{

    public float MaxHeightScreenPercent = 0.33f;

    float mMaxHeight;
    GameObject mShaft;
    private BoxCollider2D mShaftCollider;
    private float mTurretHeight;
    private float mTurretWidth = 2.0f;
    private bool mInUse;

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
        mMaxHeight = MaxHeightScreenPercent * Camera.main.orthographicSize * 2.0f;
        SetHeight(mTurretHeight);

    }

    public void SetHeight(float height)
    {
        mTurretHeight = Math.Min(height, mMaxHeight);

        // Adjust Shaft scale
        mShaft.transform.localScale = new Vector3(mTurretWidth, mTurretHeight, 0.0f);
        // Update the tiling for the turret
        mShaft.GetComponent<Renderer>().material.mainTextureScale = new Vector2(1.0f, mShaft.transform.localScale.y / 2.0f);
        // TODO - remove the actual gun height from this height. 

        // Adjust the box collider and the visible elements
        mShaftCollider.size = new Vector2(mTurretWidth, mTurretHeight);

    }

    public void Hide()
    {
        // Just moving it out of the way for now...
        this.transform.position = new Vector3(-20, -20, 0);
        enabled = false;
    }

    void Awake()
    {
        mShaft = this.transform.Find("Shaft").gameObject;
        mShaftCollider = GetComponent<BoxCollider2D>();
        
    }

    // Use this for initialization
    void Start ()
    {
        Game.GetInstance().AddScreenListener(this);
        UpdateMaxHeight();
        SetHeight(mMaxHeight);
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
        enabled = used;
        if (!used)
        {
            Hide();
        }
    }
}
