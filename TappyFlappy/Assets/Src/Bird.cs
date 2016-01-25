using UnityEngine;
using System.Collections;
using Assets.Src;

public class Bird : MonoBehaviour, IInputListener
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

    // Use this for initialization
    void Start ()
    {
        Game.GetInstance().GetInputManager().AddInputListener("flap", this);
        mCurrentVelocity = Vector3.zero;
        mIsPaused = false;
    }
	
	// Update is called once per frame
	void Update()
    {
        if (mIsPaused) 
        {
            return;
        }
        if (mIsFlying)
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
    }

    public void OnStart()
    {
        mIsFlying = true;
    }

    public void OnLose()
    {
        mIsFlying = false;
    }


}
