using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorSetActiveFalse : MonoBehaviour
{
    
    public void TurnOffDamnYou()
    {
        Debug.Log("Attempting to turn self off");
        this.gameObject.SetActive(false);
    }

}
