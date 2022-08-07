using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MyExtensions.MyExtensions;

public class SimpleFogScroller : MonoBehaviour
{
    public float scrollSpeed = 10.0f;
    public Transform fogPage1;
    public Transform fogPage2;
    public bool flipped=false;

    float _worldXsize; 
    
    
    // Start is called before the first frame update
    void Start()
    {
        _worldXsize = fogPage1.position.x * -1.0f;
    }

    void UpdateFogPagePosition(Transform page)
    {
        Vector3 position = page.position;
        position.x += scrollSpeed * Time.deltaTime;
        if (position.x > _worldXsize)
            position.x -= _worldXsize*2.0f;
        page.SetPositionAndRotation(position,Quaternion.identity);
    }
    // Update is called once per frame
    void Update()
    {
        UpdateFogPagePosition(fogPage1);
        UpdateFogPagePosition(fogPage2);
    }
}
