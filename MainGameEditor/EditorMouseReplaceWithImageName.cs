using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class EditorMouseReplaceWithImageName : MonoBehaviour
{
    String imagename;
    
    EditorReplaceMouseCursor refToMouseCursor;
    // Start is called before the first frame update
    void Awake()
    {
        refToMouseCursor = GameObject.Find("[MouseStuffs]").GetComponent<EditorReplaceMouseCursor>();
    }

    public void ReplaceMouseCursorCallWithImageName()
    {
        var imagefromButton = GetComponent<Image>();
        imagename = imagefromButton.sprite.name;
        Debug.Log($"<color=green> makes cursor {imagename} ");
        refToMouseCursor.SetnewMouseCursor(imagename);
    }
}