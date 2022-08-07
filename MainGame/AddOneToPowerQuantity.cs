using DarkTonic.MasterAudio;
using DarkTonic.PoolBoss;
using UnityEngine;
using MyExtensions;

public class AddOneToPowerQuantity : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        var player = other.GetComponent<Player>();
        if (player != null)
        {
            PowerManager.IncrementSpellQuantity();
            PoolBoss.Despawn(transform);
            MasterAudio.PlaySound("Positive Effect 1_4");
        }
    }
}