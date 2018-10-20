using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class AbilityRange : MonoBehaviour
{
    //tiles away from user (X, Z axis)
    public int horizontal = 1;

    //tile height difference (Y axis)
    public int vertical = int.MaxValue;

    //if true: base range off user direction (cone, line attack) [cursor affects direction]
    //if false: free range selection (grenade, teleport) [cursor moves within radius]
    public virtual bool directionOriented { get { return false; } }

    //reference unit to determine location
    protected Unit unit { get { return GetComponentInParent<Unit>(); } }

    //get tiles reachable by ability
    public abstract List<Tile> GetTilesInRange(Board board);

    //bool for AI use
    public virtual bool positionOriented { get { return true; } }
}