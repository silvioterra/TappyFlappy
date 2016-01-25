using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour, ISpeedListener, IScreenListener
{
    // For instantiation
    public Bullet BulletPrefab;

    private float mWorldPosition;
    private float mWorldWidth;
    private float mWorldHeight;
    private float mLastBulletSpawn;
    const float kMinTimeBetweenBullets = 0.5f;

    void Awake()
    {
        Game.GetInstance().AddScreenListener(this);
        Game.GetInstance().GetLevelManager().AddSpeedListener(this);

    }
    // Use this for initialization
    void Start ()
    {
        mLastBulletSpawn = Time.time;
        UpdateWorldVariables();
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
            if (Time.time - mLastBulletSpawn > kMinTimeBetweenBullets)
            {
                mLastBulletSpawn = Time.time;
                // FIRE THAT GUN!
                Bullet b = (Bullet)Object.Instantiate(BulletPrefab, this.transform.position, Quaternion.identity);
                Vector3 direction = (tappy.transform.position - this.transform.position)*2.0f;
                b.SetWorldDirection(direction);
                b.SetRootPosition(this.transform.position);
                b.SetWorldPosition(mWorldPosition);
                // Set the bullet's velocity.
                Game.GetInstance().GetBulletManager().AddBullet(b);
            }
        }
        
    }

    void ISpeedListener.OnSpeedChanged(float newSpeed)
    {
        // TODO 
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
