using UnityEngine;
using System.Collections;

//Conditions determine how a status effect will last once applied.
//Conditions will remove the status effect independently once finished
//E.G: end a status after set number of turns, or when hp = 0
public class StatusCondition : MonoBehaviour
{
    public virtual void Remove()
    {
        Status s = GetComponentInParent<Status>();
        if (s)
            s.Remove(this);
    }
}