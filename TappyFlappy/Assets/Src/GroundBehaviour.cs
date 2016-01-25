using UnityEngine;
using System.Collections;
using System;

public class GroundBehaviour : MonoBehaviour, IScreenListener, ISpeedListener
{
    public int mDefaultGroundHeight = 64;

    private float mTilingOffsetSpeed = 0.0f;
    void IScreenListener.OnScreenResolutionChanged(int width, int height)
    {
        UpdateGroundPosition();
    }

    void ISpeedListener.OnSpeedChanged(float newSpeed)
    {
        mTilingOffsetSpeed = newSpeed;
    }

    void ISpeedListener.OnScreenCenterPositionUpdated(float mWorldPosition)
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        float newOffset = GetComponent<Renderer>().material.mainTextureOffset.x;
        newOffset += mTilingOffsetSpeed * Time.deltaTime;
        GetComponent<Renderer>().material.mainTextureOffset = new Vector2(newOffset, 1);

    }

    void Awake()
    {

    }

    // Use this for initialization
    void Start ()
    {
        mTilingOffsetSpeed = Game.GetInstance().GetLevelManager().StartingWorldSpeed;

        UpdateGroundPosition();
        Game.GetInstance().AddScreenListener(this);
        Game.GetInstance().GetLevelManager().AddSpeedListener(this);
    }

    // Update is called once per frame
    void Update ()
    {
	    
	}

    void UpdateGroundPosition()
    {
        Camera camera = Camera.main;
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        if (meshFilter == null)
        {
            return;
        }
        Vector3 left = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0));
        Vector3 right = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0));
        Vector3 worldWidth = right - left;
        this.transform.localScale = new Vector3(worldWidth.x, 1.0f, 1.0f);
        // Update the tiling for the turret
        this.GetComponent<Renderer>().material.mainTextureScale = new Vector2(-this.transform.localScale.x, -1);

        float newY = -camera.orthographicSize + (meshFilter.transform.localScale.y * meshFilter.mesh.bounds.size.y) / 2.0f;
        Vector3 oldPosition = this.transform.position;

        this.transform.position = new Vector3(oldPosition.x, newY, oldPosition.z);

    }
}
