using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public abstract class AbilityArea : MonoBehaviour
{
    //highlight tiles within AoE range
    public abstract List<Tile> GetTilesInArea(Board board, Point pos);
}