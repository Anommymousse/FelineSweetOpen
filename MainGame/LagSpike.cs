using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LagSpike : MonoBehaviour
{
    // Start is called before the first frame update
    //float lagTime = 1.0f;
    bool lagSpikeStarted;
    bool inLagRoutine;

    void Awake()
    {
        //bool lagSpikeStarted = false;
        //bool inLagRoutine = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (lagSpikeStarted)
        {
            StartCoroutine(SpikeTheFrameRate());
        }
    }

    IEnumerator SpikeTheFrameRate()
    {
        if (inLagRoutine == true) yield return null;
        inLagRoutine = true;
        lagSpikeStarted = false;
        inLagRoutine = false;
    }
}
