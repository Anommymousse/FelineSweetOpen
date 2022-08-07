using System;
using System.Collections;
using System.Collections.Generic;
using DarkTonic.PoolBoss;
using UnityEngine;

public class DespawnOnWallOrEnemyHit : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.name != "Player")
        {
            Debug.Log($"Fireball - collision with {collision.collider.name}");
        }

        StartCoroutine(FireballDestroyed());
        
    }

    IEnumerator FireballDestroyed()
    {
        yield return null;
        PoolBoss.Despawn(gameObject.transform);
    }

    void OnDisable()
    {
        StopAllCoroutines();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
