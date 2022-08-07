using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IsCatUnlockAvailable : MonoBehaviour
{
    int framedelay = 10; 
    bool _isAvailable = false;
    public Button buttonRef;
    void Awake()
    {
        var starCount = UpdatingKittyFund.GetCurrentKittyFund();
        if (starCount > 8)
        {
            _isAvailable = true;
            buttonRef.interactable = true;
        }
        else
        {
            buttonRef.interactable = false;
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        framedelay--;
        if (framedelay < 0)
        {
            framedelay = 10;
            int starsgathered = UpdatingKittyFund.GetCurrentKittyFund();
            if (starsgathered > 8)
            {
                buttonRef.interactable = true;
            }
            else
            {
                buttonRef.interactable = false;
            }
        }
    }
}
