using UnityEngine;
using System.Collections;

//placeholder. Use to show xp and items gained before exiting battle state
public class EndBattleState : BattleState
{
    public override void Enter()
    {
        base.Enter();
        Application.LoadLevel(0);
    }
}