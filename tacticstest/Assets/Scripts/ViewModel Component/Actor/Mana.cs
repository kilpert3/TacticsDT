using UnityEngine;
using System.Collections;

public class Mana : MonoBehaviour
{
    #region Fields
    public int MP
    {
        get { return stats[StatTypes.MP]; }
        set { stats[StatTypes.MP] = value; }
    }

    public int MMP
    {
        get { return stats[StatTypes.MMP]; }
        set { stats[StatTypes.MMP] = value; }
    }

    Unit unit;
    Stats stats;
    #endregion

    //connect reference to linked unit and stats at start
    #region MonoBehaviour
    void Awake()
    {
        stats = GetComponent<Stats>();
        unit = GetComponent<Unit>();
    }

    //add notification observers (destroy when finished)
    void OnEnable()
    {
        this.AddObserver(OnMPWillChange, Stats.WillChangeNotification(StatTypes.MP), stats);
        this.AddObserver(OnMMPDidChange, Stats.DidChangeNotification(StatTypes.MMP), stats);
        this.AddObserver(OnTurnBegan, TurnOrderController.TurnBeganNotification, unit);
    }

    void OnDisable()
    {
        this.RemoveObserver(OnMPWillChange, Stats.WillChangeNotification(StatTypes.MP), stats);
        this.RemoveObserver(OnMMPDidChange, Stats.DidChangeNotification(StatTypes.MMP), stats);
        this.RemoveObserver(OnTurnBegan, TurnOrderController.TurnBeganNotification, unit);
    }
    #endregion

    //display change in mp from ability use
    #region Event Handlers
    void OnMPWillChange(object sender, object args)
    {
        ValueChangeException vce = args as ValueChangeException;
        vce.AddModifier(new ClampValueModifier(int.MaxValue, 0, stats[StatTypes.MHP]));
    }

    //apply change in mp from ability use
    void OnMMPDidChange(object sender, object args)
    {
        int oldMMP = (int)args;
        if (MMP > oldMMP)
            MP += MMP - oldMMP;
        else
            MP = Mathf.Clamp(MP, 0, MMP);
    }

    //restore 10 percent of mp at the start of each turn
    void OnTurnBegan(object sender, object args)
    {
        if (MP < MMP)
            MP += Mathf.Max(Mathf.FloorToInt(MMP * 0.1f), 1);
    }
    #endregion
}