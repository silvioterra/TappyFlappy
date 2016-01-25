using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BulletManager : IWorldStateListener, ISpeedListener, IScreenListener
{
    List<Bullet> mBullets;
    private float mWorldPosition;
    private float mWorldWidth;
    private float mWorldHeight;

    // Use this for initialization
    public BulletManager()
    {
        mBullets = new System.Collections.Generic.List<Bullet>();
        UpdateWorldVariables();

    }
	
	// Update is called once per frame
	public void Update ()
    {

        List<Bullet> delBullets = new List<Bullet>();
        foreach (Bullet b in mBullets)
        {
            b.UpdatePosition(mWorldPosition);
            Vector3 pos = b.transform.position;
            if ((pos.x < -mWorldWidth || pos.x > mWorldWidth)
                || (pos.y < -mWorldHeight || pos.y > mWorldHeight))
            {
                delBullets.Add(b);
            }
        }

        foreach(Bullet b in delBullets)
        {
            mBullets.Remove(b);
            Object.Destroy(b);
        }
    }
    void IWorldStateListener.OnRestartGame()
    {
        DestroyAllBullets();
    }

    public void DestroyAllBullets()
    {
        // Kill all bullets.
        foreach (Bullet b in mBullets)
        {
            Object.Destroy(b.gameObject);
        }
        mBullets.Clear();
    }

    internal void AddBullet(Bullet b)
    {
        mBullets.Add(b);
    }

    void IWorldStateListener.OnGameLose()
    {
        
    }

    void IWorldStateListener.OnPlayStart()
    {
        
    }

    void ISpeedListener.OnSpeedChanged(float newSpeed)
    {
        
    }

    void ISpeedListener.OnScreenCenterPositionUpdated(float worldPosition)
    {
        mWorldPosition = worldPosition;
    }

    public void OnScreenResolutionChanged(int width, int height)
    {
        UpdateWorldVariables();
    }

    private void UpdateWorldVariables()
    {
        Vector3 left = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0));
        Vector3 right = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0));
        mWorldWidth = (right - left).x;
        Vector3 bottom = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 0));
        mWorldHeight = (bottom - left).y;
    }
}
