using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DamageAbilityEffect : BaseAbilityEffect
{
    #region Public
    public override int Predict(Tile target)
    {
        Unit attacker = GetComponentInParent<Unit>();
        Unit defender = target.content.GetComponent<Unit>();

        // Get the attackers base attack stat considering
        // mission items, support check, status check, and equipment, etc
        int DamageMod = GetStat(attacker, defender, GetDamageModifierNotification, 0);

        // Get the targets base defense stat considering
        // mission items, support check, status check, and equipment, etc
        int DR = GetStat(attacker, defender, GetDamageReductionNotification, 0);

        // Get the abilities power stat considering possible variations
        int DamageRoll = GetStat(attacker, defender, GetDamageRollNotification, 0);

        // Calculate damage
        int damage = DamageMod + DamageRoll - DR ;
        damage = Mathf.Max(damage, 1);

        // Tweak the damage based on a variety of other checks like
        // Elemental damage, Critical Hits, Damage multipliers, etc.
        damage = GetStat(attacker, defender, TweakDamageNotification, damage);

        // Clamp the damage to a range
        damage = Mathf.Clamp(damage, minDamage, maxDamage);
        return -damage;
    }

    protected override int OnApply(Tile target)
    {
        Unit defender = target.content.GetComponent<Unit>();

        // Start with the predicted damage value
        int value = Predict(target);

        // Apply the damage to the target
        Stats s = defender.GetComponent<Stats>();
        s[StatTypes.HP] += value;
        return value;
    }
    #endregion
}