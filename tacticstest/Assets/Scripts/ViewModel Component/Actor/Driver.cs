using UnityEngine;
using System.Collections;

//driver determines who has control of a unit
public class Driver : MonoBehaviour
{
    public Drivers normal;
    public Drivers special;

    public Drivers Current
    {
        get
        {
            return special != Drivers.None ? special : normal;
        }
    }
}