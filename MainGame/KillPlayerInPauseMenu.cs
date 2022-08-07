using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPlayerInPauseMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public void KillPlayerAndUnpause()
    {
        InGameCountDownTimer._timerAmountLeft = 0.0f;
        Pause.UnpauseFlag = true;
    }

}
