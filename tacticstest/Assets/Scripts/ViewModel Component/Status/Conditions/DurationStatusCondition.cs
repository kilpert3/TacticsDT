using UnityEngine;
using System.Collections;

//This type of condition will last for a set number of turns. Each turn will reduce duration by 1
//E.G: poison effect, DOT effects
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

    //listen for new round notifications and reduce effect duration
    void OnNewTurn(object sender, object args)
    {
        duration--;
        if (duration <= 0)
            Remove();
    }
}