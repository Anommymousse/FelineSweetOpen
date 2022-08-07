using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class IPointerMouseButtonPressed : MonoBehaviour,IPointerClickHandler
{
    
    [SerializeField] AudioClip _audioHoverSource;
    void Start()
    {        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Mouse button pressed!");
    }

}
