using UnityEngine;
using System.Collections;
// Always around, started only once

/// <summary>
/// This serves as a main focus for the game loop, allowing for finer control over when/how other 
/// MonoBehaviours and Prefabs get spawned/Update(). Without something explicitly calling Update() on
/// certain objects, you can have behaviors updating at times when it's not safe or when other data
/// isn't ready for it. 
/// 
/// This also serves as a way for us to reference Prefabs for spawning WITHOUT needing to hardcode the names
/// in code or rely on non-dependency-checked strings.
/// 
/// This might be outdated knowledge, but it's how Unity behaved a few years ago; order of Update() calls
/// was non-deterministic.
/// </summary>
public class Game : MonoBehaviour
{
    // Once set, we can just spawn new instances of these
    protected static Game sGame;
    public static Game GetInstance() { return sGame; }

    // Use this for initialization
    void Start()
    {
        Cursor.visible = false;

        if (sGame)
        {
            // NO DUPES ALLOWED (can happen if we reload the Scene with this on the side)
            Object.DestroyImmediate(gameObject);
            return;
        }
        sGame = this;

        // keep this object around between level transitions
        Object.DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
