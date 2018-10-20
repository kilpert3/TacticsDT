using UnityEngine;
using System;
using System.Collections;

//This condition checks a stat against a boolean. It will check each turn and end the status if the bool is false.
//E.G: Use for KO status, which will last as long as HP<=0. Being healed above 0 will remove KO status
public class StatComparisonCondition : StatusCondition
{
    #region Fields
    public StatTypes type { get; private set; }
    public int value { get; private set; }
    public Func<bool> condition { get; private set; }
    Stats stats;
    #endregion

    #region MonoBehaviour
    void Awake()
    {
        stats = GetComponentInParent<Stats>();
    }

    void OnDisable()
    {
        this.RemoveObserver(OnStatChanged, Stats.DidChangeNotification(type), stats);
    }
    #endregion

    #region Public
    public void Init(StatTypes type, int value, Func<bool> condition)
    {
        this.type = type;
        this.value = value;
        this.condition = condition;
        this.AddObserver(OnStatChanged, Stats.DidChangeNotification(type), stats);
    }

    //comparison bools to use in the Init function
    public bool EqualTo()
    {
        return stats[type] == value;
    }

    public bool LessThan()
    {
        return stats[type] < value;
    }

    public bool LessThanOrEqualTo()
    {
        return stats[type] <= value;
    }

    public bool GreaterThan()
    {
        return stats[type] > value;
    }

    public bool GreaterThanOrEqualTo()
    {
        return stats[type] >= value;
    }
    #endregion

    #region Notification Handlers
    void OnStatChanged(object sender, object args)
    {
        if (condition != null && !condition())
            Remove();
    }
    #endregion
}