using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour {

	
    //repeater for cursor movement (private- only used by inputcontroller)
    class Repeater
    {
        //amount of pause between press and repeat
        const float threshold = 0.5f;
        //repeat speed
        const float rate = 0.25f;
        //time before new events register
        float _next;
        //indicate button held or not
        bool _hold;
        //store input axis
        string _axis;

        public Repeater(string axisName)
        {
            _axis = axisName;
        }

        public int Update()
        {
            int retValue = 0;
            int value = Mathf.RoundToInt(Input.GetAxisRaw(_axis));

            if (value != 0)
            {
                if (Time.time > _next)
                {
                    retValue = value;
                    _next = Time.time + (_hold ? rate : threshold);
                    _hold = true;
                }
            }
            else
            {
                _hold = false;
                _next = 0;
            }

            return retValue;
        }
    }

    Repeater _hor = new Repeater("Horizontal");
    Repeater _ver = new Repeater("Vertical");

    //report repeater events
    public static event EventHandler<InfoEventArgs<Point>> moveEvent;

    void Update()
    {
        //check repeater events
        int x = _hor.Update();
        int y = _ver.Update();
        if (x != 0 || y != 0)
        {
            if (moveEvent != null)
                moveEvent(this, new InfoEventArgs<Point>(new Point(x, y)));
        }
        //check fire events
        for (int i = 0; i < 3; ++i)
        {
            if (Input.GetButtonUp(_buttons[i]))
            {
                if (fireEvent != null)
                    fireEvent(this, new InfoEventArgs<int>(i));
            }
        }
    }

    //handle fire button presses as events
    public static event EventHandler<InfoEventArgs<int>> fireEvent;

    string[] _buttons = new string[] { "Fire1", "Fire2", "Fire3" };

   

}
