using UnityEngine;
using System.Collections;


//abstract implementation for items/equip effects
public abstract class Feature : MonoBehaviour
{
    #region Fields / Properties
    protected GameObject _target { get; private set; }
    #endregion

    #region Public
    //apply stat change when item equip
    public void Activate(GameObject target)
    {
        if (_target == null)
        {
            _target = target;
            OnApply();
        }
    }

    //undo stat change when item unequip
    public void Deactivate()
    {
        if (_target != null)
        {
            OnRemove();
            _target = null;
        }
    }

    //permanent change (one time use items)
    public void Apply(GameObject target)
    {
        _target = target;
        OnApply();
        _target = null;
    }
    #endregion

    #region Private
    protected abstract void OnApply();
    protected virtual void OnRemove() { }
    #endregion
}