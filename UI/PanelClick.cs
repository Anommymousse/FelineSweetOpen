using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelClick : MonoBehaviour
{
    Color _originalColor;
    Image _updateSubPanel;
    static bool isActive;
    public void OnClickThePanel()
    {
        var menusubcanvas = GetComponentInChildren<IdentifySubPanel>();
        if (menusubcanvas != null)
        {     
            _updateSubPanel = menusubcanvas.GetComponent<Image>();
            if (_updateSubPanel != null)
            {
                if (isActive == false)
                {
                    isActive = true;
                    StartCoroutine(CycleColor());
                }
            }
        }
    }

    IEnumerator CycleColor()
    {        
        _originalColor = _updateSubPanel.color;
        Color color = _originalColor;
        color.r = color.r / 2;
        color.g = color.g / 2;
        color.b = color.b / 2;
        _updateSubPanel.color = color;
        yield return null;
        yield return new WaitForSeconds(0.5f);
        _updateSubPanel.color = _originalColor;
        yield return null;
        isActive = false;
    }
}

    
