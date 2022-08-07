using System;
using System.Collections;
using System.Collections.Generic;
using DarkTonic.PoolBoss;
using UnityEngine;

public class DespawnOnTouch : MonoBehaviour
{
    bool willAttemptToDestroyBrick = true;
    BrickMap _brickMapRef;
    MoveConstantSpeed _moveConstantSpeedRef;

    void Awake()
    {
        _brickMapRef = GameObject.Find("TilesBoss").GetComponent<BrickMap>();
        _moveConstantSpeedRef = gameObject.GetComponent<MoveConstantSpeed>();
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.name.Contains("Character")) return;
        
        PoolBoss.Despawn(this.transform);
        if (willAttemptToDestroyBrick)
        {
            //_bricksRef.DestroyBrick( collision.GetContact(0).point);
            _brickMapRef.DestroyBrick(_moveConstantSpeedRef.AdjustColliderCauseUnityCantReturnACollisionCorrectly() + collision.GetContact(0).point);
        }
    }
}
