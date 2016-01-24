using UnityEngine;
using Assets.Src;
using System.Collections;

public class GroundBehaviour : MonoBehaviour, IScreenListener
{
    public int mDefaultGroundHeight = 64;
    void IScreenListener.OnScreenResolutionChanged(int width, int height)
    {
        UpdateGroundPosition();
    }

    // Use this for initialization
    void Start ()
    {
        UpdateGroundPosition();
        Game.GetInstance().AddScreenListener(this);
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    void UpdateGroundPosition()
    {
        Camera camera = Camera.main;
        MeshFilter meshFilter = (MeshFilter)GetComponent("MeshFilter");
        if (meshFilter == null)
        {
            return;
        }

        float newY = -camera.orthographicSize + (meshFilter.transform.localScale.y * meshFilter.mesh.bounds.size.y) / 2.0f;
        Vector3 oldPosition = this.transform.position;

        this.transform.position = new Vector3(oldPosition.x, newY, oldPosition.z);
    }
}
