using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour, IWorldStateListener
{
    // For instantiation
    public Bullet BulletPrefab;

    System.Collections.Generic.List<Bullet> mBullets;

    void IWorldStateListener.OnGameLose()
    {
        // Kill bullets?
        // oooooorrrrrr keep firing bullets?
    }

    void IWorldStateListener.OnPlayStart()
    {
    }

    void IWorldStateListener.OnRestartGame()
    {
        // Kill all bullets.
        foreach(Bullet b in mBullets)
        {
            Destroy(b.gameObject);
        }
        mBullets.Clear();
    }

    void Awake()
    {
        mBullets = new System.Collections.Generic.List<Bullet>();
    }
    // Use this for initialization
    void Start ()
    {
        Game.GetInstance().AddWorldStateListener(this);
    }

    // Update is called once per frame
    void Update ()
    {
	
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        Bird tappy = other.gameObject.GetComponent<Bird>();
        if (tappy != null)
        {
            Debug.Log("Gun Triggered");
            // FIRE THAT GUN!
            Bullet b = (Bullet)Object.Instantiate(BulletPrefab, this.transform.position, Quaternion.identity);


            // Set the bullet's velocity.
            // TODO - this really needs to kill the bullet if it gets out of the screen.
            mBullets.Add(b);
        }
        
    }
}
