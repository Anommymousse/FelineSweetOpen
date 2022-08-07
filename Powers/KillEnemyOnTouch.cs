using System;
using System.Collections;
using System.Collections.Generic;
using DarkTonic.PoolBoss;
using UnityEngine;
using static MyExtensions.MyExtensions;
public class KillEnemyOnTouch : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Character"))
        {
            //Log($" found in layer {other.gameObject.name}");
            PoolBoss.Despawn(other.gameObject.transform);
        }
        else
        {
            //Log($" NOT found in layer {other.gameObject.name} layer {other.gameObject.layer}");
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.gameObject.layer == LayerMask.NameToLayer("Character"))
        {
            PoolBoss.Despawn(other.collider.gameObject.transform);
        }
    }
}