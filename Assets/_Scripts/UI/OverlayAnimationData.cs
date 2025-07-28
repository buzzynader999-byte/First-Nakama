using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class OverlayAnimationData
{
    [Header("Open Animation")]
    public bool useOpenAnimation = false;
    public RectTransform[] openElements;
    public float[] openDurations = new float[] { 0.3f };
    public LeanTweenType[] openEaseTypes = new LeanTweenType[] {LeanTweenType.easeOutQuart};
    public float openDelay = 0.1f;

    [Header("Close Animation")]
    public bool useCloseAnimation = false;
    public RectTransform[] closeElements;
    public float[] closeDurations = new float[] { 0.3f };
    public LeanTweenType[] closeEaseTypes = new LeanTweenType[] { LeanTweenType.easeOutQuart };
    public float closeDelay = 0.1f;
}
