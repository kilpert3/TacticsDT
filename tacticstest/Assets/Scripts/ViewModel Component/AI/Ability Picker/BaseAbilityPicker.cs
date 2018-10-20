using UnityEngine;
using System.Collections;

//abstract base class. Allows for abilities to be chosen by the AI.
public abstract class BaseAbilityPicker : MonoBehaviour
{
    //reference to unit and its abilities
    #region Fields
    protected Unit owner;
    protected AbilityCatalog ac;
    #endregion

    #region MonoBehaviour
    void Start()
    {
        owner = GetComponentInParent<Unit>();
        ac = owner.GetComponentInChildren<AbilityCatalog>();
    }
    #endregion

    //implement Pick method to determine how abilities are chosen.
    #region Public
    public abstract void Pick(PlanOfAttack plan);
    #endregion

    //find an ability that matches a string name
    #region Protected
    protected Ability Find(string abilityName)
    {
        for (int i = 0; i < ac.transform.childCount; ++i)
        {
            Transform category = ac.transform.GetChild(i);
            Transform child = category.Find(abilityName);
            if (child != null)
                return child.GetComponent<Ability>();
        }
        return null;
    }

    protected Ability Default()
    {
        return owner.GetComponentInChildren<Ability>();
    }
    #endregion
}