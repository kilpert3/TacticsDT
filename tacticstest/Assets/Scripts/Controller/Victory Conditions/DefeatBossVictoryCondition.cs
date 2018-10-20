using UnityEngine;
using System.Collections;

//victory occurs when a target unit has been defeated
//E.G: the boss is defeated
public class DefeatBossVictoryCondition : BaseVictoryCondition
{
    public Unit target;

    protected override void CheckForGameOver()
    {
        base.CheckForGameOver();
        if (Victor == Factions.None && IsDefeated(target))
            Victor = Factions.Hero;
    }
}