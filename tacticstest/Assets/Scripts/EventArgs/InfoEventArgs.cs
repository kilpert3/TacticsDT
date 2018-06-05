using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InfoEventArgs<T> : EventArgs
{
    //simple event arg holder
    public T info;

    //default constructor
    public InfoEventArgs()
    {
        info = default(T);
    }

    //user specified info type constructor
    public InfoEventArgs(T info)
    {
        this.info = info;
    }
}
