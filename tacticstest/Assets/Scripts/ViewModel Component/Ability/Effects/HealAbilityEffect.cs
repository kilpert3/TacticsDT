using UnityEngine;
using System.Collections;

//uses power stat
public class HealAbilityEffect : BaseAbilityEffect
{
    public override int Predict(Tile target)
    {
        Unit attacker = GetComponentInParent<Unit>();
        Unit defender = target.content.GetComponent<Unit>();
        return GetStat(attacker, defender, GetPowerNotification, 0);
    }

    protected override int OnApply(Tile target)
    {
        Unit defender = target.content.GetComponent<Unit>();

        // Start with the predicted value
        int value = Predict(target);

        // Generate RNG
        value = Mathf.FloorToInt(value * UnityEngine.Random.Range(0.9f, 1.1f));

        // Clamp the amount to the heal range
        value = Mathf.Clamp(value, minDamage, maxDamage);

        // Apply heal on target
        Stats s = defender.GetComponent<Stats>();
        s[StatTypes.HP] += value;
        return value;
    }
}