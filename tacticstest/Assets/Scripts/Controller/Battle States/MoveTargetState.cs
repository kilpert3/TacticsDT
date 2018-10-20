﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;


//ENTER: Highlight squares for unit movement
//EXIT: Move location chosen
public class MoveTargetState : BattleState
{
    List<Tile> tiles;

    public override void Enter()
    {
        base.Enter();
        Movement mover = turn.actor.GetComponent<Movement>();
        tiles = mover.GetTilesInRange(board);
        board.SelectTiles(tiles);
        RefreshPrimaryStatPanel(pos);

        if (driver.Current == Drivers.Computer)
            StartCoroutine(ComputerHighlightMoveTarget());
    }

    public override void Exit()
    {
        base.Exit();
        board.DeSelectTiles(tiles);
        tiles = null;
        statPanelController.HidePrimary();
    }

    protected override void OnMove(object sender, InfoEventArgs<Point> e)
    {
        SelectTile(e.info + pos);
        RefreshPrimaryStatPanel(pos);
    }

    
    protected override void OnFire(object sender, InfoEventArgs<int> e)
    {
        if (e.info == 0)
        {
            if (tiles.Contains(owner.currentTile))
                owner.ChangeState<MoveSequenceState>();
        }
        else //allow movement to cancel
        {
            owner.ChangeState<CommandSelectionState>();
        }
    }

    //shows Computer Player cursor movement
    IEnumerator ComputerHighlightMoveTarget()
    {
        Point cursorPos = pos;
        while (cursorPos != turn.plan.moveLocation)
        {
            if (cursorPos.x < turn.plan.moveLocation.x) cursorPos.x++;
            if (cursorPos.x > turn.plan.moveLocation.x) cursorPos.x--;
            if (cursorPos.y < turn.plan.moveLocation.y) cursorPos.y++;
            if (cursorPos.y > turn.plan.moveLocation.y) cursorPos.y--;
            SelectTile(cursorPos);
            yield return new WaitForSeconds(0.25f);
        }
        yield return new WaitForSeconds(0.5f);
        owner.ChangeState<MoveSequenceState>();
    }
}