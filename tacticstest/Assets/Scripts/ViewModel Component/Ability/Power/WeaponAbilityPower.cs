using UnityEngine;
using System.Collections;


//implementation uncertain

public class WeaponAbilityPower : BaseAbilityPower
{
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
        int damage = 0;
        Equipment eq = GetComponentInParent<Equipment>();
        Equippable item = eq.GetItem(EquipSlots.Primary);

        //use STR mod for unarmed hits
        if (item == null)
            return GetBaseDamageModifier();

        //add weapon base damage roll
        WeaponBase weapon = item.gameObject.GetComponent<WeaponBase>();
        for (int i = 0; i < weapon.rolls; i++)
            damage += Random.Range(1, weapon.d);

        //add damage from additional item effects
        StatModifierFeature[] features = item.GetComponentsInChildren<StatModifierFeature>();

        for (int i = 0; i < features.Length; ++i)
        {
            if (features[i].type == StatTypes.STR)
                damage += features[i].amount;
        }

        return damage;
    }

}