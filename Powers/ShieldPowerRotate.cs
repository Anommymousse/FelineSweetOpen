using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ShieldPowerRotate : MonoBehaviour
{
    void Start()
    {
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        gameObject.transform.Rotate(2.3f,3.2f,-5.6f);
        
    }
}
