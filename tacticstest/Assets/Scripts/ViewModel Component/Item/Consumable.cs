using UnityEngine;
using System.Collections;

public class Consumable : MonoBehaviour
{
    //apply item features to target of consumable item
    public void Consume(GameObject target)
    {
        Feature[] features = GetComponentsInChildren<Feature>();
        for (int i = 0; i < features.Length; ++i)
            features[i].Apply(target);
    }
}