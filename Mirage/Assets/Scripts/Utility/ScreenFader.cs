﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// fades ui component on/off
public class ScreenFader : MonoBehaviour
{
    // alpha value for opaqueness
    [SerializeField]
    protected float _solidAlpha = 1f;

    // alpah value for transparency
    [SerializeField]
    protected float _clearAlpha = 0f;

    // time to fade on
    [SerializeField]
    private float _fadeOnDuration = 2f;
    public float FadeOnDuration { get { return _fadeOnDuration; } }

    // time to fade off
    [SerializeField]
    private float _fadeOffDuration = 2f;
    public float FadeOffDuration { get { return _fadeOffDuration; } }

    // all graphics that need to fade on/off
    [SerializeField]
    private MaskableGraphic[] graphicsToFade;

    // sets the graphics to the given alpha value
    protected void SetAlpha(float alpha)
    {
        foreach (MaskableGraphic graphic in graphicsToFade)
        {
            if (graphic != null)
            {
                graphic.canvasRenderer.SetAlpha(alpha);
            }
        }
    }

    // changes the alpha of graphics to target value over duration
    private void Fade(float targetAlpha, float duration)
    {
        //Debug.Log("Fading");
        foreach (MaskableGraphic graphic in graphicsToFade)
        {
            if (graphic != null)
            {
                graphic.CrossFadeAlpha(targetAlpha, duration, true);
            }
        }
    }
    
    // fade from clear to opaque
    public void FadeOn()
    {
        SetAlpha(_clearAlpha);
        Fade(_solidAlpha, _fadeOnDuration);
    }

    // fade from opaque to clear
    public void FadeOff()
    {
        SetAlpha(_solidAlpha);
        Fade(_clearAlpha, _fadeOffDuration);
    }


}
