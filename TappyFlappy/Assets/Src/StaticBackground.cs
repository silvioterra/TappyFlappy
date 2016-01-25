using UnityEngine;
using System.Collections;
using System;

public class StaticBackground : MonoBehaviour , IScreenListener
{

	// Use this for initialization
	void Start ()
    {
        Game.GetInstance().AddScreenListener(this);
        ResizeBackground();
    }

    void ResizeBackground()
    {
        SpriteRenderer spriteRenderer = (SpriteRenderer)GetComponent("SpriteRenderer");
        if (spriteRenderer == null)
        {
            return;
        }
        transform.localScale.Set(1.0f, 1.0f, 1.0f);
        float height = spriteRenderer.sprite.bounds.size.y;

        float screenHeight = Camera.main.orthographicSize * 2.0f;
        float screenWidth = screenHeight / Screen.height * Screen.width;

        // First, try to scale the Y to match the visible height.
        float yScale = screenHeight / height;

        // Oh right, Unity is crazy. This won't work
        // Because... hah.. ha.... why, again? 
        // transform.localScale.Set(yScale, yScale, 1.0f);

        transform.localScale = new Vector3(yScale, yScale, 1.0f);

        // Then, if the X scale (which should match Y scale) isn't enough to cover the sides,
        // stretch JUST the X scale until it covers everything. This case should only occur with 
        // SEVERE aspect ratios (resizing windows) in comparison to the original artwork, which I'm assuming
        // should cover most common aspect ratios. A more scalable solution would be to have the
        // background simply render again, tiled. I'm sure there's a way to adjust the UVs of a sprite
        // in Unity like that, I simply lack the experience and time to find this right now.
        // One solution I've found is to turn this into an actual 3D object (cube, for instance) and adjust the
        // tiling of the texture on the assigned material. That works GREAT and I'm going to do that for the ground
        // because I really want to tile that. This is just a generic blue gradient background and I don't care if it stretches.
        float width = spriteRenderer.sprite.bounds.size.x;
        if (width < screenWidth)
        {
            float xScale = screenWidth / width;
            transform.localScale = new Vector3(xScale, yScale, 1.0f);
        }
    }

    // Update is called once per frame
    void Update ()
    {
    }

    void IScreenListener.OnScreenResolutionChanged(int width, int height)
    {
        ResizeBackground();
    }
}
