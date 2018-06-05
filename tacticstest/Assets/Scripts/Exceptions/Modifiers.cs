using UnityEngine;
using System.Collections;

//abstract class for stat modifiers. Concrete subclasses in Modifiers folder
public abstract class Modifier
{
    public readonly int sortOrder;

    public Modifier(int sortOrder)
    {
        this.sortOrder = sortOrder;
    }
}