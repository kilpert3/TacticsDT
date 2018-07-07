using UnityEngine;
using System.Collections;

//reveals attacker/defender combination that caused an error
public class MatchException : BaseException
{
    public readonly Unit attacker;
    public readonly Unit target;

    public MatchException(Unit attacker, Unit target) : base(false)
    {
        this.attacker = attacker;
        this.target = target;
    }
}