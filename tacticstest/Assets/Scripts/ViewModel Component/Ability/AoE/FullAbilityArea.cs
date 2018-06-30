using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//target everything in range (cone of cold, beam attack)
public class FullAbilityArea : AbilityArea
{
    public override List<Tile> GetTilesInArea(Board board, Point pos)
    {
        AbilityRange ar = GetComponent<AbilityRange>();
        return ar.GetTilesInRange(board);
    }
}