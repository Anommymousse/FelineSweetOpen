using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Pause;

public class UnPauseGame : MonoBehaviour
{
    // Start is called before the first frame update
    public void SetUnPauseFlag()
    {
        Pause.UnpauseFlag = true;
    }

    
}
