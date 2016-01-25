using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Assets.Src;

// This clearly isn't a very robust InputManager, but enough for a 1-day jam
// Normally I'd have a system InputManager and the game registering a certain set of 
// inputs to certain actions in the game. So the GAME would never ask "is the 'E' key down", but
// instead it would ask "is 'Use' active", which the InputManager would translate as "ok, 'Use' 
// is 'E' but has an alt control scheme of 'R' or, if in gamepad mode, 'cross'. Any of those down?"
// 
// I mean, standard stuff. I just don't have a Unity library ready to plop in here for this :)
// So this will be combining game + system input behavior.
public class InputManager
{
    // Just supporting 2 button mice right now.
    const int kNumMouseButtons = 2;
    bool[] mMouseDown;

    bool mKeyDown;


    Dictionary<string, List<IInputListener>> mInputListeners;

    public InputManager()
    {

        mMouseDown = new bool[kNumMouseButtons];
        mKeyDown = false;
        mInputListeners = new Dictionary<string, List<IInputListener>>();
    }

    // Update is called once per frame
    public void Update()
    {
        for (int i = 0; i < kNumMouseButtons; i++)
        {
            if (Input.GetMouseButtonDown(i))
            {
                if (mMouseDown[i])
                {
                    // Hm. Ok, so we CANNOT trust the GetMouseButtonDown function...
                    System.Console.WriteLine("Use plain GetMouseButton instead, Unity isn't treating clicks correctly due to alt-tab or whatever.");
                }
                else
                {
                    mMouseDown[i] = true;
                }
                DispatchInputEvent("flap");
            }
            if (Input.GetMouseButtonUp(i))
            {
                if (!mMouseDown[i])
                {
                    // Hm. Ok, so we CANNOT trust the GetMouseButtonDown function...
                    System.Console.WriteLine("Use plain GetMouseButton instead, Unity isn't treating clicks correctly due to alt-tab or whatever.");
                }
                else
                {
                    mMouseDown[i] = false;
                }
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (mKeyDown)
                {
                    // Hm. Ok, so we CANNOT trust the GetMouseButtonDown function...
                    System.Console.WriteLine("Use plain GetKey instead, Unity isn't treating spacebar correctly due to alt-tab or whatever.");
                }
                else
                {
                    mKeyDown = true;
                }
                DispatchInputEvent("flap");
            }
            if (Input.GetKeyUp(KeyCode.Space))
            {
                if (!mKeyDown)
                {
                    // Hm. Ok, so we CANNOT trust the GetMouseButtonDown function...
                    System.Console.WriteLine("Use plain GetKey instead, Unity isn't treating spacebar correctly due to alt-tab or whatever.");
                }
                else
                {
                    mKeyDown = false;
                }
            }
        }
    }
    private void DispatchInputEvent(string inputName)
    {
        if (mInputListeners.ContainsKey(inputName))
        {
            foreach(IInputListener listener in mInputListeners[inputName])
            {
                listener.OnInputTriggered(inputName);
            }
        }
    }

    // For 'Any input', just give it an empty string.
    public void AddInputListener(string inputName, IInputListener listener)
    {
        if (!mInputListeners.ContainsKey(inputName))
        {
            mInputListeners.Add(inputName, new List<IInputListener>());
        }

        if(!mInputListeners[inputName].Contains(listener))
        {
            mInputListeners[inputName].Add(listener);
        }
    }

    // TODO - remove input listener

}

