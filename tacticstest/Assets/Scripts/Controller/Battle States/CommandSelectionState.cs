﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//PREV: SelectUnitState
//choose from list of turn commands
//NEXT: CategorySelectionState
public class CommandSelectionState : BaseAbilityMenuState
{
    // the first menu display when selecting a unit. (move/action/wait)
    protected override void LoadMenu()
    {
        if (menuOptions == null)
        {
            menuTitle = "Commands";
            menuOptions = new List<string>(3);
            menuOptions.Add("Move");
            menuOptions.Add("Action");
            menuOptions.Add("Wait");
        }

        abilityMenuPanelController.Show(menuTitle, menuOptions);
        abilityMenuPanelController.SetLocked(0, turn.hasUnitMoved);
        abilityMenuPanelController.SetLocked(1, turn.hasUnitActed);
    }

    protected override void Confirm()
    {
        switch (abilityMenuPanelController.selection)
        {
            case 0: // Move
                owner.ChangeState<MoveTargetState>();
                break;
            case 1: // Action
                owner.ChangeState<CategorySelectionState>();
                break;
            case 2: // Wait
                owner.ChangeState<EndFacingState>();
                break;
        }
    }

    //will either undo a move or exit the menu to explore state
    protected override void Cancel()
    {
        if (turn.hasUnitMoved && !turn.lockMove)
        {
            turn.UndoMove();
            abilityMenuPanelController.SetLocked(0, false);
            SelectTile(turn.actor.tile.pos);
        }
        else
        {
            owner.ChangeState<ExploreState>();
        }
    }

    //make sure open stat panels are closed when done
    public override void Enter()
    {
        base.Enter();
        statPanelController.ShowPrimary(turn.actor.gameObject);

        if(turn.actor.gameObject.GetComponentInChildren<Animator>() != null)
            turn.actor.gameObject.GetComponentInChildren<Animator>().SetTrigger("hoverOver");

        //activate AI for enemy units
        if (driver.Current == Drivers.Computer)
            StartCoroutine(ComputerTurn());
    }

    public override void Exit()
    {
        base.Exit();
        statPanelController.HidePrimary();
    }

    //routine for computer controlled units
    IEnumerator ComputerTurn()
    {
        if (turn.plan == null)
        {
            turn.plan = owner.cpu.Evaluate();
            turn.ability = turn.plan.ability;
        }

        yield return new WaitForSeconds(1f);

        if (turn.hasUnitMoved == false && turn.plan.moveLocation != turn.actor.tile.pos)
            owner.ChangeState<MoveTargetState>();
        else if (turn.hasUnitActed == false && turn.plan.ability != null)
            owner.ChangeState<AbilityTargetState>();
        else
            owner.ChangeState<EndFacingState>();
    }
}