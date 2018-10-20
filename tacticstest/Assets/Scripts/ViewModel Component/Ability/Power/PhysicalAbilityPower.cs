using UnityEngine;
using System.Collections;

public class PhysicalAbilityPower : BaseAbilityPower
{
    public int rolls;
    public int d;

    protected override int GetBaseDamageModifier()
    {
        return GetComponentInParent<Stats>().getModifier(StatTypes.STR);
    }

    protected override int GetBaseDamageReduction(Unit target)
    {
        return target.GetComponent<Stats>()[StatTypes.DR];
    }

    protected override int GetDamageRoll()
    {
        int total = 0;
        for (int i = 0; i < rolls; i++)
            total += Random.Range(1, d);
        return total;
    }
}