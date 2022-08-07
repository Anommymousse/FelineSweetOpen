using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelRightTogglePosition : MonoBehaviour
{
    [SerializeField] GameObject panelBubble;
    [SerializeField] GameObject panelMain;
    bool _panelInuse;
    static bool alreadyAdjusting = false;

    void Start()
    {
        if (alreadyAdjusting == false)
        {
       //     StartCoroutine(AdjustBubblePanelPosition());
        }
    }

    /*IEnumerator AdjustBubblePanelPosition()
    {
        alreadyAdjusting = true;
        
        var position = panelBubble.transform.position;
        position.x -= 2000f;
        panelBubble.transform.position = position;
        yield return new WaitForSeconds(1.0f);
        
        alreadyAdjusting = false;
    }*/

    public void RightTogglePanel()
    {
        if (_panelInuse)
        {
            var position =panelBubble.transform.localPosition;
            position.x -= 2000f;
            panelBubble.transform.localPosition = position;
            
            position = panelMain.transform.position;
            position.x += 2000f;
            panelMain.transform.position = position;
            
        }
        else
        {
            var position =panelBubble.transform.localPosition;
            position.x += 2000f;
            panelBubble.transform.localPosition = position;
            
            position = panelMain.transform.position;
            position.x -= 2000f;
            panelMain.transform.position = position;
        }

        _panelInuse = !_panelInuse;
    }
    
}
