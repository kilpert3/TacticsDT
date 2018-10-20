using UnityEngine;
using System.Collections;

//Victory occurs when all enemies have been defeated
public class DefeatAllVictoryCondition : BaseVictoryCondition
{
    protected override void CheckForGameOver()
    {
        base.CheckForGameOver();
        if (Victor == Factions.None && PartyDefeated(Factions.Enemy))
            Victor = Factions.Hero;
    }
}