using DarkTonic.PoolBoss;
using UnityEngine;
using static MyExtensions.MyExtensions;

public class KillEnemiesIncludingElectricOnTouch : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Character")|| other.gameObject.layer == LayerMask.NameToLayer("Non Collidable Dynamic Obstacle") )
        {
            if(other.name.Contains("Pirate")) GenericUnlockAchievement.UnlockAchievement("PirateLife");
            PoolBoss.Despawn(other.gameObject.transform);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.gameObject.layer == LayerMask.NameToLayer("Character"))
        {
            if(other.collider.name.Contains("Pirate")) GenericUnlockAchievement.UnlockAchievement("PirateLife");
            PoolBoss.Despawn(other.collider.gameObject.transform);
        }
    }
}