using UnityEngine;
using System.Collections;
using System;

public class Bird : MonoBehaviour, IInputListener, IWorldStateListener
{

    // Vector3 for simplicity of other calls, Vector2 should be enough
    Vector3 mCurrentVelocity;
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
        mCurrentVelocity = Vector3.zero;
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
        if (mIsFlying && !mUseUnityPhysics)
        {
            Vector3 currentPosition = transform.position;
            // Update the current velocity with gravity

            // this makes some new allocations. Set() would be more efficient but less legible right now.
            Vector3 frameForcesPerSecond = mGravityPerSecond;

            mCurrentVelocity += frameForcesPerSecond * Time.deltaTime;

            if (mCurrentVelocity.sqrMagnitude > (mMaxVelocityPerSecond * mMaxVelocityPerSecond))
            {
                mCurrentVelocity.Normalize();
                mCurrentVelocity *= mMaxVelocityPerSecond;
            }

            currentPosition += mCurrentVelocity * Time.deltaTime;

            transform.position = currentPosition;
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
            OnStart();
        }
        Flap();
    }

    public void Flap()
    {
        // Override all other powers to FLAP, FLAP YOU MONSTER!
        mCurrentVelocity = mFlapVector;


        mPhysicsBody.velocity = mCurrentVelocity;
    }

    public void OnStart()
    {
        mIsFlying = true;
        mPhysicsBody.simulated = true;
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
    }
}
