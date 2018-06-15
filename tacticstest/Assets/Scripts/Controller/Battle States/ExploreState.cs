using UnityEngine;
using System.Collections;

//state for player to roam map with cursor (no unit selected)
public class ExploreState : BattleState
{
    
    protected override void OnMove(object sender, InfoEventArgs<Point> e)
    {
        SelectTile(e.info + pos);
        RefreshPrimaryStatPanel(pos);
    }

    protected override void OnFire(object sender, InfoEventArgs<int> e)
    {
        if (e.info == 0)
            owner.ChangeState<CommandSelectionState>();
    }
}