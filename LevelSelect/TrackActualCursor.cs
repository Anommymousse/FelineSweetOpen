using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackActualCursor : MonoBehaviour
{
    GameObject cursorRef;
    // Start is called before the first frame update
    void Start()
    {
        cursorRef = GameObject.Find("Cursor");
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = cursorRef.transform.position;
    }
}
