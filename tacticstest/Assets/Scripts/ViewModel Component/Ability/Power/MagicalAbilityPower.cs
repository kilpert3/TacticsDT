using UnityEngine;
using System.Collections;

public class MagicalAbilityPower : BaseAbilityPower
{
    public int rolls;
    public int d;
    public int flatMod;

    public bool useIntMod;

    protected override int GetBaseDamageModifier()
    {
        if (useIntMod)
            return GetComponentInParent<Stats>().getModifier(StatTypes.INT) + flatMod;

        return flatMod;
    }

    protected override int GetBaseDamageReduction(Unit target)
    {
        return target.GetComponent<Stats>()[StatTypes.MR];
    }

    protected override int GetDamageRoll()
    {
        int total = 0;
        for (int i = 0; i <rolls; i++)
            total += Random.Range(1, d);
        return total;
    }
}