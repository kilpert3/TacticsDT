using UnityEngine;
using System.Collections;


//load spreadsheet data, listen for level up notifications, apply stat changes
public class Job : MonoBehaviour
{
    //convenience array for parsing spreadsheet
    #region Fields / Properties
    public static readonly StatTypes[] statOrder = new StatTypes[]
    {
    StatTypes.MHP,
    StatTypes.MMP,
    StatTypes.ATK,
    StatTypes.DEF,
    StatTypes.MAT,
    StatTypes.MDF,
    StatTypes.SPD
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
    }
    #endregion

    #region Event Handlers
    protected virtual void OnLvlChangeNotification(object sender, object args)
    {
        int oldValue = (int)args;
        int newValue = stats[StatTypes.LVL];

        for (int i = oldValue; i < newValue; ++i)
            LevelUp();
    }
    #endregion

    #region Private
    void LevelUp()
    {
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