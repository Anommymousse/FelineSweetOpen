using System;
using UnityEngine;
using UnityEngine.UI;

public class EditorMouseReplaceWithRealtimeButtonImage : MonoBehaviour
{
    EditorReplaceMouseCursor refToMouseCursor;
    // Start is called before the first frame update
    void Awake()
    {
        refToMouseCursor = GameObject.Find("[MouseStuffs]").GetComponent<EditorReplaceMouseCursor>();
    }

    public void ReplaceMouseCursorCallWithImageName()
    {
        var imagefromButton = GetComponent<Image>();
        var imagename = imagefromButton.sprite.name;
        Debug.Log($"name of sprite = {imagename} ");
        refToMouseCursor.SetnewMouseCursor(imagename);
    }
}