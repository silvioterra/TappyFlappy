using UnityEngine;
using System.Collections;
using System;

public class Turret : MonoBehaviour, IScreenListener
{

    public float MaxHeightScreenPercent = 0.33f;

    float mMaxHeight;

    void IScreenListener.OnScreenResolutionChanged(int width, int height)
    {
        UpdateMaxHeight();
    }

    private void UpdateMaxHeight()
    {
        mMaxHeight = MaxHeightScreenPercent * Screen.height;
    }

    // Use this for initialization
    void Start ()
    {
        UpdateMaxHeight();
        Game.GetInstance().AddScreenListener(this);
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}
}
