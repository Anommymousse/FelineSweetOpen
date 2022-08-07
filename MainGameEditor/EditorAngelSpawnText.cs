using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EditorAngelSpawnText : MonoBehaviour
{
    EditorScanTileMapsForTilename _editorRef;
    Image _imageRef;
    void OnEnable()
    {
        _editorRef = GameObject.Find("Checks").GetComponent<EditorScanTileMapsForTilename>();
        _editorRef.OnAngelDespawn += OnDespawn;
        _editorRef.OnAngelSpawnSet += OnSpawnSet;
        _imageRef = GetComponent<Image>();
    }

    void OnSpawnSet()
    {
        //var tmptext = GetComponent<TMP_Text>();
        _imageRef.color = Color.white;
        //Debug.Log("Spawn Set");
    }

    void OnDespawn()
    {
        _imageRef.color = Color.red;
        //Debug.Log("UNdespawn Set");
    }

    void OnDisable()
    {
        _editorRef.OnAngelDespawn -= OnDespawn;
        _editorRef.OnAngelSpawnSet -= OnSpawnSet;

    }
}