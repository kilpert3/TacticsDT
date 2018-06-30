using UnityEngine;
using System.Collections;

//determine whether an effect applies to unit at specified tile
public abstract class AbilityEffectTarget : MonoBehaviour
{
    public abstract bool IsTarget(Tile tile);
}