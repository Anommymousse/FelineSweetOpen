using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollTheFog : MonoBehaviour
{
    [SerializeField] bool IgnoreResize;
    [SerializeField] bool IsTrailing;
    [SerializeField] double scale = 1.0;
    [SerializeField] float incrementHorizontal=1;
    float currentHorizontal_x;
    int backgroundwidth;
    RectTransform _rectTransform;

    // Start is called before the first frame update
    void Start()
    {
        
        backgroundwidth = (int) ((double) Screen.width * scale);
        
        _rectTransform = GetComponent <RectTransform>();
        
        if (IsTrailing ==true)
        {
            currentHorizontal_x = -backgroundwidth;
        }
        else
        {
            currentHorizontal_x = 0;
        }

        if (IgnoreResize == false)
        {
            _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Screen.width);
            _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Screen.height);
            _rectTransform.ForceUpdateRectTransforms();
        }

    }

    // Update is called once per frame
    void Update()
    {
        //var position =_rectTransform.position;
        currentHorizontal_x += incrementHorizontal*Time.deltaTime;
        
        var localposition = _rectTransform.localPosition;
        localposition.x = currentHorizontal_x;
        if (localposition.x >= backgroundwidth)
        {
            localposition.x = -backgroundwidth;
            currentHorizontal_x = -backgroundwidth;
        }
        ;
        //var worldpos =  _rectTransform.TransformPoint(localposition);
        _rectTransform.localPosition = localposition;
        //_rectTransform.position = worldpos;
        _rectTransform.ForceUpdateRectTransforms();
    }
}
