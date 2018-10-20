using UnityEngine;
using System.Collections;

public enum StatTypes
{
    LVL, // Level
    EXP, // Experience
    HP,  // Hit Points
    MHP, // Max Hit Points
    MP,  // Magic Points
    MMP, // Max Magic Points
    FOC, // Focus, magic points regenerated per round

    ATK, // Bonus to hit rolls
    DR,  // Damage Resistance
    MR,  // Magic Resistance
    EVD, // Evasion

    STR, // Strength
    AGL, // Agility
    END, // Endurance
    INT, // Intelligence
    RES, // Resistance
    CHA, // Charisma

    MOV, // Move Range
    JMP, // Jump Height
    INIT,// Turn Initiative
    
    SKL, // Skill Points

    Count
}