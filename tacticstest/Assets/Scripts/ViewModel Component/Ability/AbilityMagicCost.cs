using UnityEngine;
using System.Collections;

//regulate abilities that cost mp
public class AbilityMagicCost : MonoBehaviour
{
    #region Fields
    public int amount; //mp cost of ability
    Ability owner;
    #endregion

    #region MonoBehaviour
    void Awake()
    {
        owner = GetComponent<Ability>();
    }

    void OnEnable()
    {
        this.AddObserver(OnCanPerformCheck, Ability.CanPerformCheck, owner);
        this.AddObserver(OnDidPerformNotification, Ability.DidPerformNotification, owner);
    }

    void OnDisable()
    {
        this.RemoveObserver(OnCanPerformCheck, Ability.CanPerformCheck, owner);
        this.RemoveObserver(OnDidPerformNotification, Ability.DidPerformNotification, owner);
    }
    #endregion

    #region Notification Handlers
    //disable ability toggle if there is not enough mp to cast
    void OnCanPerformCheck(object sender, object args)
    {
        Stats s = GetComponentInParent<Stats>();
        if (s[StatTypes.MP] < amount)
        {
            BaseException exc = (BaseException)args;
            exc.FlipToggle();
        }
    }

    //reduce mp by the cost
    void OnDidPerformNotification(object sender, object args)
    {
        Stats s = GetComponentInParent<Stats>();
        s[StatTypes.MP] -= amount;
    }
    #endregion
}