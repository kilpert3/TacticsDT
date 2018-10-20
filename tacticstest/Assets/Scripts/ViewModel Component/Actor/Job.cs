using UnityEngine;
using System.Collections;


//load spreadsheet data, listen for level up notifications, apply stat changes
public class Job : MonoBehaviour
{
    //convenience array for parsing spreadsheet
    #region Fields / Properties
    public static readonly StatTypes[] statOrder = new StatTypes[]
    {
        StatTypes.MHP,  //1
        StatTypes.MMP,  //2
        StatTypes.FOC,  //3
        StatTypes.STR,  //4
        StatTypes.AGL,  //5
        StatTypes.END,  //6
        StatTypes.INT,  //7
        StatTypes.RES,  //8
        StatTypes.CHA,  //9
        StatTypes.MOV,
        StatTypes.JMP,
        StatTypes.SKL
    };

    public int[] baseStats = new int[statOrder.Length];
    public float[] growStats = new float[statOrder.Length];
    Stats stats;
    #endregion

    #region MonoBehaviour
    void OnDestroy()
    {
        this.RemoveObserver(OnLvlChangeNotification, Stats.DidChangeNotification(StatTypes.LVL));
    }
    #endregion

    #region Public
    //gets reference to actor’s Stats component so we can listen for target level up notifications and apply growths in response.
    //also allows any job-based features to become active.
    public void Employ()
    {
        stats = gameObject.GetComponentInParent<Stats>();
        this.AddObserver(OnLvlChangeNotification, Stats.DidChangeNotification(StatTypes.LVL), stats);

        Feature[] features = GetComponentsInChildren<Feature>();
        for (int i = 0; i < features.Length; ++i)
            features[i].Activate(gameObject);
    }

    //deactivate the jobs features and unregister from level up notifications etc. [Do this before switching]
    public void UnEmploy()
    {
        Feature[] features = GetComponentsInChildren<Feature>();
        for (int i = 0; i < features.Length; ++i)
            features[i].Deactivate();

        this.RemoveObserver(OnLvlChangeNotification, Stats.DidChangeNotification(StatTypes.LVL), stats);
        stats = null;
    }

    //initialize stats to job values. [Use when making units for the first time]
    public void LoadDefaultStats()
    {
        for (int i = 0; i < statOrder.Length; ++i)
        {
            StatTypes type = statOrder[i];
            stats.SetValue(type, baseStats[i], false);
        }

        stats.SetValue(StatTypes.HP, stats[StatTypes.MHP], false);
        stats.SetValue(StatTypes.MP, stats[StatTypes.MMP], false);
        //stats.SetValue(StatTypes.EVD, (8 + stats.getModifier(StatTypes.AGL)), false);
        //stats.SetValue(StatTypes.MR, stats.getModifier(StatTypes.RES), false);
        //stats.SetValue(StatTypes.ATK, 1 + stats.getModifier(StatTypes.STR) + stats.getModifier(StatTypes.AGL), false);
    }
    #endregion

    #region Event Handlers
    protected virtual void OnLvlChangeNotification(object sender, object args)
    {
        int oldValue = (int)args;
        int newValue = stats[StatTypes.LVL];

        LevelUp();
    }
    #endregion

    #region Private
    void LevelUp()
    {
        //placeholder implementation

        //growth stats are floats. The whole number is a guaranteed increase, the decimal is a % chance for +1 increase.
        //E.G 2.5 = +2 guaranteed, with 50% chance for +3 instead
        for (int i = 0; i < statOrder.Length; ++i)
        {
            StatTypes type = statOrder[i];
            int whole = Mathf.FloorToInt(growStats[i]);
            float fraction = growStats[i] - whole;

            int value = stats[type];
            value += whole;
            if (UnityEngine.Random.value > (1f - fraction))
                value++;

            stats.SetValue(type, value, false);
        }

        stats.SetValue(StatTypes.HP, stats[StatTypes.MHP], false);
        stats.SetValue(StatTypes.MP, stats[StatTypes.MMP], false);
    }
    #endregion
}