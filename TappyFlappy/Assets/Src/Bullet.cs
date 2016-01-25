using UnityEngine;
using System.Collections;
using System;

public class Bullet : MonoBehaviour {

    public Vector3 WorldDirection;
    public Vector3 RelativePosition;
    public Vector3 RootPosition;
    private float mWorldPosition;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	    
	}

    internal void SetWorldDirection(Vector3 direction)
    {
        WorldDirection = direction; 
    }

    public void UpdatePosition(float worldPosition)
    {
        float diff = mWorldPosition - worldPosition;
        RelativePosition = RelativePosition + (WorldDirection * Time.deltaTime);
        this.transform.position = new Vector3(RelativePosition.x + diff + RootPosition.x, RelativePosition.y + RootPosition.y, RelativePosition.z + RootPosition.z);
    } 

    internal void SetRootPosition(Vector3 position)
    {
        RootPosition = position;
    }

    internal void SetWorldPosition(float worldPosition)
    {
        mWorldPosition = worldPosition;
    }
}
