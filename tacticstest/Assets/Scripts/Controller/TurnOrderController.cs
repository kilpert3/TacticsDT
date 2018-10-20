﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TurnOrderController : MonoBehaviour
{

    #region Constants
    const int turnActivation = 1000;
    const int turnCost = 500;
    const int moveCost = 300;
    const int actionCost = 200;
    #endregion

    #region Notifications
    public const string RoundBeganNotification = "TurnOrderController.roundBegan";
    public const string TurnCheckNotification = "TurnOrderController.turnCheck";
    public const string TurnCompletedNotification = "TurnOrderController.turnCompleted";
    public const string RoundEndedNotification = "TurnOrderController.roundEnded";
    public const string TurnBeganNotification = "TurnOrderController.TurnBeganNotification";
    #endregion

    public DiceBag diceBag;
    public BattleController bc;
    public List<Unit> units;

    
    private void Start()
    {
        diceBag = new DiceBag();  
    }

    //no coroutine - step by completing combat rounds
    #region Public
    public IEnumerator Round()
    {
        BattleController bc = GetComponent<BattleController>(); ;
        while (true)
        {
            this.PostNotification(RoundBeganNotification);

            List<Unit> units = new List<Unit>(bc.units);
            for (int i = 0; i < units.Count; ++i)
            {
                Stats s = units[i].GetComponent<Stats>();
                s[StatTypes.INIT] += s[StatTypes.AGL];
            }

            units.Sort((a, b) => GetInitiative(a).CompareTo(GetInitiative(b)));

            for (int i = units.Count - 1; i >= 0; --i)
            {
                if (CanTakeTurn(units[i]))
                {
                    bc.turn.Change(units[i]);
                    units[i].PostNotification(TurnBeganNotification);

                    yield return units[i];

                    int cost = turnCost;
                    if (bc.turn.hasUnitMoved)
                        cost += moveCost;
                    if (bc.turn.hasUnitActed)
                        cost += actionCost;

                    Stats s = units[i].GetComponent<Stats>();
                    s.SetValue(StatTypes.INIT, s[StatTypes.INIT] - cost, false);

                    units[i].PostNotification(TurnCompletedNotification);
                }
            }

            this.PostNotification(RoundEndedNotification);
        }
    }
    #endregion

    #region Private
    bool CanTakeTurn(Unit target)
    {
        BaseException exc = new BaseException(GetInitiative(target) >= turnActivation);
        target.PostNotification(TurnCheckNotification, exc);
        return exc.toggle;
    }

    int GetInitiative(Unit target)
    {
        return target.GetComponent<Stats>()[StatTypes.INIT];
    }
    #endregion
}