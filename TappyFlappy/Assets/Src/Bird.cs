using UnityEngine;
using System.Collections;
using System;

public class Bird : MonoBehaviour, IInputListener, IWorldStateListener
{

    // Vector3 for simplicity of other calls, Vector2 should be enough
    private bool mIsPaused;

    // By default, pointing straight down. Wind, etc, might want to affect this?
    public Vector3 mGravityPerSecond;
    // By default, pointing straight up (opposite and greater than gravity)
    public Vector3 mFlapVector;

    public float mMaxVelocityPerSecond = 15.0f;
    private bool mIsFlying;

    // Alternatively, use the Unity physics for all this. 
    // Might not be as flexible for crazy stuff, but it saves a lot of time
    private bool mUseUnityPhysics = true;
    Rigidbody2D mPhysicsBody;
    private bool mPlaying = true;
    private int mInputLockFrames;

    // Use this for initialization
    void Start ()
    {
        Game.GetInstance().GetInputManager().AddInputListener("flap", this);

        Game.GetInstance().AddWorldStateListener(this);
        mIsPaused = false;
        mInputLockFrames = 0;

        mPhysicsBody = (Rigidbody2D)GetComponent("Rigidbody2D");
        mPhysicsBody.simulated = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (mIsPaused) 
        {
            return;
        }
        if (mInputLockFrames > 0)
        {
            mInputLockFrames--;
        }

        // Map initial flap velocity to 60 and terminal velocity to -90
        if (mIsFlying)
        {

            float rotZ = 0;
            if (mPhysicsBody.velocity.y > 0)
            {
                // 60 <-> 0

                float pct = mPhysicsBody.velocity.y / mFlapVector.y;
                rotZ = pct * 60;
            }
            else if (mPhysicsBody.velocity.y < 0)
            {
                // Relying on only ever changing the y vector for bird
                float pct = Math.Min(1, -mPhysicsBody.velocity.y / mMaxVelocityPerSecond);

                rotZ = pct * -90;
            }
            transform.localRotation = Quaternion.Euler(0, 0, rotZ);
        }
    }

    void IInputListener.OnInputTriggered(string name)
    {
        if (!mPlaying || mInputLockFrames > 0)
        {
            // Nope, gotta restart first.
            return;
        }
        // we're only listening for one input anyway.
        if (!mIsFlying)
        {
            Game.GetInstance().StartFlapping();
        }
        // We also flap, regardless
        Flap();
    }

    public void Flap()
    {
        // Override all other powers to FLAP, FLAP YOU MONSTER!

        mPhysicsBody.velocity = mFlapVector;
    }

    public void OnLose()
    {
        mIsFlying = false;
        mPhysicsBody.simulated = false;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("collision_die"))
        {
            // Oh no game over man.
            OnLose();
            Game.GetInstance().ShowLoseScreen();
        }

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // TODO - bird becomes worried if the other is a gun.

    }
    void IWorldStateListener.OnGameLose()
    {
        // TODO - display dead bird?
        mPlaying = false;
    }

    void IWorldStateListener.OnRestartGame()
    {
        mInputLockFrames = 5;
        mPlaying = true;
        // Reset bird's position!
        transform.position = new Vector3(0, 0, 0);
        transform.localRotation = Quaternion.Euler(0, 0, 0);
        mPhysicsBody.angularVelocity = 0;
    }

    void IWorldStateListener.OnPlayStart()
    {
        mIsFlying = true;
        mPhysicsBody.simulated = true;
    }
}
