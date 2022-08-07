using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateInSprial : MonoBehaviour
{
    SpriteRenderer _spriteRenderer;
    Vector3 rotations;
    
    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        rotations = _spriteRenderer.transform.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        rotations.z += 25.0f * Time.deltaTime;
        _spriteRenderer.transform.eulerAngles = rotations;
    }
}
