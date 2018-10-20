using UnityEngine;
using System.Collections;

//AI will choose the ability designated by string.
//if the ability is unavailable, will default to first ability found (attack)
public class FixedAbilityPicker : BaseAbilityPicker
{
    public Targets target;
    public string ability;

    public override void Pick(PlanOfAttack plan)
    {
        plan.target = target;
        plan.ability = Find(ability);

        if (plan.ability == null)
        {
            plan.ability = Default();
            plan.target = Targets.Foe;
        }
    }
}