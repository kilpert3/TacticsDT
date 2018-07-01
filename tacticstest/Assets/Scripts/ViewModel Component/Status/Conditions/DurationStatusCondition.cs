using UnityEngine;
using System.Collections;

//listen for new round notifications and reduce effect duration
public class DurationStatusCondition : StatusCondition
{
    //use to specify round duration
    public int duration = 10;

    void OnEnable()
    {
        this.AddObserver(OnNewTurn, TurnOrderController.RoundBeganNotification);
    }

    void OnDisable()
    {
        this.RemoveObserver(OnNewTurn, TurnOrderController.RoundBeganNotification);
    }

    void OnNewTurn(object sender, object args)
    {
        duration--;
        if (duration <= 0)
            Remove();
    }
}