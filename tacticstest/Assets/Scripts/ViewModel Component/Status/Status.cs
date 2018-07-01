using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//determine how long the status effect is applied to a unit:
//checks for the existance of status conditions.
//As long as there is a condition tied to an effect, the effect remains applied
public class Status : MonoBehaviour
{
    public const string AddedNotification = "Status.AddedNotification";
    public const string RemovedNotification = "Status.RemovedNotification";

    public U Add<T, U>() where T : StatusEffect where U : StatusCondition
    {
        T effect = GetComponentInChildren<T>();

        if (effect == null)
        {
            effect = gameObject.AddChildComponent<T>();
            this.PostNotification(AddedNotification, effect);
        }

        return effect.gameObject.AddChildComponent<U>();
    }

    public void Remove(StatusCondition target)
    {
        //unparent before destroying (objects are not destroyed instantly, which could cause ref problems)
        StatusEffect effect = target.GetComponentInParent<StatusEffect>();

        target.transform.SetParent(null);
        Destroy(target.gameObject);

        StatusCondition condition = effect.GetComponentInChildren<StatusCondition>();
        if (condition == null)
        {
            effect.transform.SetParent(null);
            Destroy(effect.gameObject);
            this.PostNotification(RemovedNotification, effect);
        }
    }
}