using UnityEngine;
using System.Collections;

public class Faction : MonoBehaviour
{
    public Factions type;

    //possible status condition
    public bool confused;

    public bool IsMatch(Faction other, Targets targets)
    {
        bool isMatch = false;
        switch (targets)
        {
            case Targets.Self:
                isMatch = other == this;
                break;
            case Targets.Ally:
                isMatch = type == other.type;
                break;
            case Targets.Foe:
                isMatch = (type != other.type) && other.type != Factions.Neutral;
                break;
        }
        return confused ? !isMatch : isMatch;
    }
}