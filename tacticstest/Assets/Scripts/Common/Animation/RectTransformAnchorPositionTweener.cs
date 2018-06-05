using UnityEngine;
using System.Collections;


//maintains a reference to its own RectTransform and sets the interpolated Vector (from the update loop) as the anchoredPosition value.

public class RectTransformAnchorPositionTweener : Vector3Tweener
{
    RectTransform rt;

    protected override void Awake()
    {
        base.Awake();
        rt = transform as RectTransform;
    }

    protected override void OnUpdate(object sender, System.EventArgs e)
    {
        base.OnUpdate(sender, e);
        rt.anchoredPosition = currentValue;
    }
}