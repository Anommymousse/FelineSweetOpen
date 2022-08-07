using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CopyTMPTextToChildren : MonoBehaviour
{
    [SerializeField] string TextToPlace;
    // Start is called before the first frame update
    void OnValidate()
    {
        var _TMPtextArray = GetComponentsInChildren<TMP_Text>();
        foreach (var textEntry in _TMPtextArray)
        {
            textEntry.SetText(TextToPlace);
        }
    }
}
