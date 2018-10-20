using UnityEngine;
using System.Collections;

//uses power stat
public class HealAbilityEffect : BaseAbilityEffect
{
    public override int Predict(Tile target)
    {
        Unit attacker = GetComponentInParent<Unit>();
        Unit defender = target.content.GetComponent<Unit>();
        return GetStat(attacker, defender, GetDamageRollNotification, 0);
    }

    protected override int OnApply(Tile target)
    {
        Unit defender = target.content.GetComponent<Unit>();

        // Start with the predicted value
        int value = Predict(target);

        // Clamp the amount to range
        value = Mathf.Clamp(value, minDamage, maxDamage);

        // Apply heal on target
        Stats s = defender.GetComponent<Stats>();
        s[StatTypes.HP] += value;
        return value;
    }
}