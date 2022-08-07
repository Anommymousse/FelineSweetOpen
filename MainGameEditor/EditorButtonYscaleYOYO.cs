using System;
using System.Collections;
using System.Collections.Generic;
using DarkTonic.MasterAudio;
using DG.Tweening;
using UnityEngine;

public class EditorButtonYscaleYOYO : MonoBehaviour
{
    bool _inCoroutine;

    void Start()
    {
        _inCoroutine= false;
    }

    public void UX_ButtonPressedScaleY()
    {
        if (_inCoroutine == false)
            StartCoroutine(DoScaleOfButton());
    }

    IEnumerator DoScaleOfButton()
    {
        yield return null;
        MasterAudio.PlaySound("BUTTON_Click_Compressor_Small_02_stereo");
        transform.DOScaleX(1.05f,0.3f);
        transform.DOScaleY(1.15f,0.3f);
        yield return new WaitForSeconds(0.1f);
        transform.DOScaleX(1f,0.2f);
        transform.DOScaleY(1f,0.2f);
        yield return new WaitForSeconds(0.2f);
        _inCoroutine = false;
    }
}
