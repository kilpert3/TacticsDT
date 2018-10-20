using UnityEngine;
using System.Collections;

//ability effects only enemy units
public class EnemyAbilityEffectTarget : AbilityEffectTarget
{
    Faction faction;

    void Start()
    {
        faction = GetComponentInParent<Faction>();
    }

    public override bool IsTarget(Tile tile)
    {
        if (tile == null || tile.content == null)
            return false;

        Faction other = tile.content.GetComponentInChildren<Faction>();
        return faction.IsMatch(other, Targets.Foe);
    }
}