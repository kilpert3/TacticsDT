using UnityEngine;
using System.Collections;

//'hold person' status. allows guaranteed hits on afflicted target
public class StopStatusEffect : StatusEffect
{
    Stats myStats;

    void OnEnable()
    {
        myStats = GetComponentInParent<Stats>();
        this.AddObserver(OnAutomaticHitCheck, HitRate.AutomaticHitCheckNotification);
        if (myStats)
            this.AddObserver(OnCounterWillChange, Stats.WillChangeNotification(StatTypes.CTR), myStats);
    }

    void OnDisable()
    {
        this.RemoveObserver(OnAutomaticHitCheck, HitRate.AutomaticHitCheckNotification);
        this.RemoveObserver(OnCounterWillChange, Stats.WillChangeNotification(StatTypes.CTR), myStats);
    }

    void OnCounterWillChange(object sender, object args)
    {
        ValueChangeException exc = args as ValueChangeException;
        exc.FlipToggle();
    }

    void OnAutomaticHitCheck(object sender, object args)
    {
        Unit owner = GetComponentInParent<Unit>();
        MatchException exc = args as MatchException;
        if (owner == exc.target)
            exc.FlipToggle();
    }
}