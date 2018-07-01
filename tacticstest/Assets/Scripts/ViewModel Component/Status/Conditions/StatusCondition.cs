using UnityEngine;
using System.Collections;

//acts as lock for status effects. handles removal of self when status ends
public class StatusCondition : MonoBehaviour
{
    public virtual void Remove()
    {
        Status s = GetComponentInParent<Status>();
        if (s)
            s.Remove(this);
    }
}