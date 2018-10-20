using UnityEngine;
using System.Collections;


//base class to determine the conditions for ending a scenario
//E.G: all allies/enemies defeated, boss defeated, item recovered
public abstract class BaseVictoryCondition : MonoBehaviour
{
    //the property shown to other classes (who won the scenario)
    public Factions Victor
    {
        get { return victor; }
        protected set { victor = value; }
    }
    Factions victor = Factions.None;

    //reference battle controller
    protected BattleController bc;

    protected virtual void Awake()
    {
        bc = GetComponent<BattleController>();
    }

    //listen for hp change notifications
    protected virtual void OnEnable()
    {
        this.AddObserver(OnHPDidChangeNotification, Stats.DidChangeNotification(StatTypes.HP));
    }

    protected virtual void OnDisable()
    {
        this.RemoveObserver(OnHPDidChangeNotification, Stats.DidChangeNotification(StatTypes.HP));
    }

    protected virtual void OnHPDidChangeNotification(object sender, object args)
    {
        CheckForGameOver();
    }

    //check for a units hp reaching min value (unit defeated)
    protected virtual bool IsDefeated(Unit unit)
    {
        Health health = unit.GetComponent<Health>();
        if (health)
            return health.MinHP == health.HP;

        Stats stats = unit.GetComponent<Stats>();
        return stats[StatTypes.HP] == 0;
    }

    //check if any parties have been entirely defeated
    protected virtual bool PartyDefeated(Factions type)
    {
        for (int i = 0; i < bc.units.Count; ++i)
        {
            Faction a = bc.units[i].GetComponent<Faction>();
            if (a == null)
                continue;

            if (a.type == type && !IsDefeated(bc.units[i]))
                return false;
        }
        return true;
    }

    //Game over if player party is defeated
    protected virtual void CheckForGameOver()
    {
        if (PartyDefeated(Factions.Hero))
            Victor = Factions.Enemy;
    }
}