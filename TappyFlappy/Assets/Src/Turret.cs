using UnityEngine;
using System.Collections;
using System;

public class Turret : Obstacle
{
    private float mTurretHeight = 1.0f;
    private BoxCollider2D mHackCollider;
    GameObject mShaft;
    private BoxCollider2D mShaftCollider;
    float mMaxHeight;
    private GameObject mGun;

    protected override void UpdateMaxHeight()
    {
        base.UpdateMaxHeight();
        SetHeight(mTurretHeight);

    }


    public void SetHeight(float height)
    {
        mTurretHeight = Math.Min(height, mMaxHeight) * mWorldHeight;

        // Adjust Shaft scale
        mShaft.transform.localScale = new Vector3(mWidth, mTurretHeight, 0.0f);
        // Update the tiling for the turret
        mShaft.GetComponent<Renderer>().material.mainTextureScale = new Vector2(1.0f, mTurretHeight / 2.0f);
        // TODO - remove the actual gun height from this height. 

        mHackCollider.size = new Vector2(mWidth, 1.0f);
        mGun.transform.localScale = new Vector3(mWidth, 1.0f, 1.0f);
        // Assuming mGun's scale is always 1.0, not going to change this.
        mGun.transform.localPosition = new Vector3(0, mTurretHeight / 2.0f + 0.5f, 0);
        // Adjust the box collider and the visible elements
        mShaftCollider.size = new Vector2(mWidth, mTurretHeight);

    }
    protected override void SetWorldPosition(float xPosition)
    {
        mPositionInWorld = new Vector2(xPosition, mTurretHeight);
    }

    protected override void InternalAwake()
    {
        base.InternalAwake();
        mMaxHeight = MaxHeightScreenPercent;// Camera.main.orthographicSize * 2.0f;

        mShaft = this.transform.Find("Shaft").gameObject;
        mGun = this.transform.Find("Gun").gameObject;
        mHackCollider = this.transform.Find("Shaft/InfinityTube").gameObject.GetComponent<BoxCollider2D>();
        
        mShaftCollider = GetComponent<BoxCollider2D>();

    }
}
