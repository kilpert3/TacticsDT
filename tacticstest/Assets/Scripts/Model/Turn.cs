using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Turn
{
    public Unit actor; //the selected unit

    //allow for undo move action
    public bool hasUnitMoved;
    public bool hasUnitActed;
    public bool lockMove; 

    //initial tile and direction facing
    Tile startTile;
    Directions startDir;

    public Ability ability; //the selected ability
    public List<Tile> targets; //list of targets within ability range

    public PlanOfAttack plan; //plan used by AI units

    public void Change(Unit current)
    {
        actor = current;
        hasUnitMoved = false;
        hasUnitActed = false;
        lockMove = false;
        startTile = actor.tile;
        startDir = actor.dir;
        plan = null;
    }

    public void UndoMove()
    {
        hasUnitMoved = false;
        actor.Place(startTile);
        actor.dir = startDir;
        actor.Match();
    }
}