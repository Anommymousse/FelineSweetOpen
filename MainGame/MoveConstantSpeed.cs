using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveConstantSpeed : MonoBehaviour
{
    public float speed = 0.1f;
    Vector2 travelDirection= Vector2.left;

    public void SetDirection(Vector2 directionOfTravel)
    {
        travelDirection = directionOfTravel;
    }

    public Vector2 AdjustColliderCauseUnityCantReturnACollisionCorrectly()
    {
        Vector2 adjDirection = travelDirection * 0.4f;
        return adjDirection;
    }

    void FixedUpdate()
    {
        var newDelta = speed * travelDirection;
        gameObject.transform.position += (Vector3)newDelta;
    }
}
