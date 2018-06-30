using UnityEngine;
using System.Collections;

public class PerformAbilityState : BattleState
{
    public override void Enter()
    {
        base.Enter();
        turn.hasUnitActed = true;
        if (turn.hasUnitMoved)
            turn.lockMove = true;
        StartCoroutine(Animate());
    }

   IEnumerator Animate()
    {
        // TODO play animations, etc
        yield return null;
        // TODO apply ability effect, etc
        TemporaryAttackExample();

        if (turn.hasUnitMoved)
            owner.ChangeState<EndFacingState>();
        else
            owner.ChangeState<CommandSelectionState>();
    }
    
    //PLACEHOLDER
    //in the future, do not directly do the work of an Ability’s Effect in this state.
    //Instead, have another class per effect, similar to the implementation of item Feature components.
    //have the final implementation loop through the effects and targets and attempt to apply the effect on each target.
    void TemporaryAttackExample()
    {
        for (int i = 0; i < turn.targets.Count; ++i)
        {
            GameObject obj = turn.targets[i].content;
            Stats stats = obj != null ? obj.GetComponentInChildren<Stats>() : null;
            if (stats != null)
            {
                stats[StatTypes.HP] -= 50;
                if (stats[StatTypes.HP] <= 0)
                    Debug.Log("KO'd Uni!", obj);
            }
        }
    }
}