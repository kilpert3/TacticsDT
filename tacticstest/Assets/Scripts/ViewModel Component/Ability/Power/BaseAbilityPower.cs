using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//listen for ability effect notifications, then adjust appropriate modifier
public abstract class BaseAbilityPower : MonoBehaviour
{
    protected abstract int GetBaseDamageModifier();
    protected abstract int GetBaseDamageReduction(Unit target);
    protected abstract int GetDamageRoll();

    void OnEnable()
    {
        this.AddObserver(OnGetBaseAttack, DamageAbilityEffect.GetDamageModifierNotification);
        this.AddObserver(OnGetBaseDefense, DamageAbilityEffect.GetDamageReductionNotification);
        this.AddObserver(OnGetPower, DamageAbilityEffect.GetDamageRollNotification);
    }

    void OnDisable()
    {
        this.RemoveObserver(OnGetBaseAttack, DamageAbilityEffect.GetDamageModifierNotification);
        this.RemoveObserver(OnGetBaseDefense, DamageAbilityEffect.GetDamageReductionNotification);
        this.RemoveObserver(OnGetPower, DamageAbilityEffect.GetDamageRollNotification);
    }

    void OnGetBaseAttack(object sender, object args)
    {
        var info = args as Info<Unit, Unit, List<ValueModifier>>;
        if (info.arg0 != GetComponentInParent<Unit>())
            return;

        AddValueModifier mod = new AddValueModifier(0, GetBaseDamageModifier());
        info.arg2.Add(mod);
    }

    void OnGetBaseDefense(object sender, object args)
    {
        var info = args as Info<Unit, Unit, List<ValueModifier>>;
        if (info.arg0 != GetComponentInParent<Unit>())
            return;

        AddValueModifier mod = new AddValueModifier(0, GetBaseDamageReduction(info.arg1));
        info.arg2.Add(mod);
    }

    void OnGetPower(object sender, object args)
    {
        var info = args as Info<Unit, Unit, List<ValueModifier>>;
        if (info.arg0 != GetComponentInParent<Unit>())
            return;

        AddValueModifier mod = new AddValueModifier(0, GetDamageRoll());
        info.arg2.Add(mod);
    }
}