using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillThePlayer : MonoBehaviour
{

    void OnCollisionEnter2D(Collision2D collision)
    {
        var player = collision.collider.GetComponent<Player>();
        if (player != null) 
        {
            Debug.Log($"Sub Call to boop the player");
            player.KillThePlayer(gameObject.name);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
        {
            var player = collision.GetComponent<Player>();
            if (player != null)
            {
                Debug.Log($"Sub Call to boop the player");
                player.KillThePlayer(gameObject.name);
            }
        }

    void OnParticleCollision(GameObject other)
    {
        var player = other.GetComponent<Player>();
        if (player != null)
        {
            Debug.Log($"Sub Call to boop the player");
            player.KillThePlayer(gameObject.name);
        }
    }
    
}
