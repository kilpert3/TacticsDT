using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class SpeakerData
{
    public List<string> messages;
    public Sprite speaker;
    //which corner the panel will show up in
    public TextAnchor anchor;
}