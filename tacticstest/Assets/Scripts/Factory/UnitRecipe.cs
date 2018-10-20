using UnityEngine;
using System.Collections;

//basic unit template
public class UnitRecipe : ScriptableObject
{
    public string model;
    public string job;
    public string attack;
    public string abilityCatalog;
    public Locomotions locomotion;
    public Factions faction;
    public string strategy;
}