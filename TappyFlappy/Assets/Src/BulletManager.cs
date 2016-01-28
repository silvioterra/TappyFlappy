using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BulletManager : IWorldStateListener, ISpeedListener, IScreenListener
{
    List<Bullet> mBullets;
	List<Bullet> mDeleteBullets;
    private float mWorldPosition;
    private float mWorldWidth;
    private float mWorldHeight;

    // Use this for initialization
    public BulletManager()
    {
        mBullets = new System.Collections.Generic.List<Bullet>();
		mDeleteBullets= new System.Collections.Generic.List<Bullet>();
        UpdateWorldVariables();

    }
	
	// Update is called once per frame
	public void Update ()
    {
		mDeleteBullets.Clear ();

        
		for(int i = 0; i< mBullets.Count; i++)
		{
			Bullet b = mBullets [i];
            b.UpdatePosition(mWorldPosition);
            Vector3 pos = b.transform.position;
            if ((pos.x < -mWorldWidth || pos.x > mWorldWidth)
                || (pos.y < -mWorldHeight || pos.y > mWorldHeight))
            {
                mDeleteBullets.Add(b);
            }
        }
		for(int i = 0; i< mDeleteBullets.Count; i++)
		{
			Bullet b = mDeleteBullets [i];
            mBullets.Remove(b);
			Object.Destroy(b.gameObject);
        }
    }
    void IWorldStateListener.OnRestartGame()
    {
        DestroyAllBullets();
    }

    public void DestroyAllBullets()
    {
        // Kill all bullets.
		for(int i = 0; i< mBullets.Count; i++)
		{
			Bullet b = mBullets [i];
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
