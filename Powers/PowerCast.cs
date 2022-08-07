using System.Collections;
using System.Collections.Generic;
using DarkTonic.MasterAudio;
using DarkTonic.PoolBoss;
using UnityEngine;

public class PowerCast : MonoBehaviour
{
    public static void CastsFireball(Vector3 castPosition,bool isGoingLeft)
    {
        var fireball = PoolBoss.SpawnInPool("PlayerFireball", castPosition, Quaternion.identity);
        if (fireball != null)
        {
            var fireballProjectile = fireball.GetComponent<FireballProjectile>();
            if (isGoingLeft)
            {
                Vector3 offsetForLeft = new Vector3(-0.5f,0.5f,0.0f);
                fireballProjectile.SetDirection(-1.0f);
                fireballProjectile.AdjustOffset(offsetForLeft);
            }
            else
            {
                Vector3 offsetForRight = new Vector3(0.5f,0.5f,0.0f);
                fireballProjectile.AdjustOffset(offsetForRight);
                fireballProjectile.SetDirection(1.0f);
            }
            
            fireballProjectile.SetSpeed(0.2f);
            Debug.Log("Spawn fireball");
            MasterAudio.PlaySound("FireballSpellCast");
        }
        else
        {
            Debug.Log("<color=red> Can't spawn due to poolboss </color>");
        }
    }
    
    public static void CastsLightning(Vector3 position, bool isLeft)
    {
        float xlength = 10;
        var transformpos = position;
        var lightning = PoolBoss.SpawnInPool("PlayerLightning", transformpos, Quaternion.identity);
        if (lightning == null) return;
        
        var particleSys = lightning.GetComponent<ParticleSystem>();
        
        //Offsets and setup for rotation of particle.
        var adjustTransform = transformpos;
        adjustTransform.y += 0.5f;
        var wallhit = RayCastToHitWall(adjustTransform, isLeft);
        Vector3 offset;
        float rotationZ;
        
        Debug.Log($"<color=green> wallhit {wallhit} </color>");
        
        xlength = wallhit.x - transformpos.x;
        xlength = Mathf.Abs(xlength);
        var newposition = particleSys.transform.position;
        if (isLeft)
        {
            offset = new Vector3(0.0f, 0.5f, 0.0f);
            rotationZ = Mathf.PI;
            newposition.x -= xlength;
        }
        else
        {
            offset = new Vector3(0.0f, 0.5f, 0.0f);
            rotationZ = 0f;
            newposition.x += xlength;
        }
        newposition += offset;
        particleSys.transform.position = newposition;
        var mainParticleSystemModule = particleSys.main;
        mainParticleSystemModule.startSizeX = new ParticleSystem.MinMaxCurve(xlength);
        var thing = mainParticleSystemModule.startRotation3D;
        mainParticleSystemModule.startRotationZ =  rotationZ;
        
        particleSys.Play();

        MasterAudio.PlaySound("LightningSpellCast");
        
        //Hit and destroy enemies
        LightningStrikeDestroyEnemies(adjustTransform,isLeft,xlength);
        
        
        Debug.Log("Spawn lightning");
    }

    static void LightningStrikeDestroyEnemies(Vector3 transformpos,bool isLeft,float length)
    {
        //Was Enemies
        var _collisionLayermask = (1<< LayerMask.NameToLayer("Character"));
        RaycastHit2D[] enemyHitList = new RaycastHit2D[20];
        Vector2 direction = Vector2.right;
        if(isLeft) direction = Vector2.left;

        var endpoint = transformpos;
        endpoint.x += direction.x * length;
        
        Debug.DrawLine(transformpos,endpoint,Color.green);
        
        var test = Physics2D.RaycastNonAlloc(transformpos,direction,enemyHitList, length,_collisionLayermask);
        if (test > 0)
        {
            for(int i = 0; i<test;i++)
            {
                if (enemyHitList[i].collider != null)
                {
                    Debug.Log($"Lightning hit {enemyHitList[i].collider.gameObject.name}");
                    PoolBoss.Despawn(enemyHitList[i].collider.gameObject.transform);
                    MasterAudio.PlaySound("EnemyAshDeath");
                }
            }
        }
        
    }
    
    static Vector3 RayCastToHitWall(Vector3 startposition, bool direction)
    {
        Vector2 spellDirection;
        
        if (direction == true)
        {
            spellDirection = Vector3.left;
        }
        else
        {
            spellDirection = Vector3.right;
        }
        
        Vector3 spellDirection3d = spellDirection;
        var endposition = startposition + spellDirection3d * 36.0f;
        Debug.DrawLine(startposition,endposition,Color.blue);
        
        // send out a ray to see what gets hit
        var startthing = startposition;
        
        var _collisionLayermask = (1<< LayerMask.NameToLayer("Bricks"));
        
        var test = Physics2D.Raycast(startposition, spellDirection, 40.0f,_collisionLayermask);
        
        var point = (Vector2)startthing + (test.distance) * spellDirection;

        return point;
    }


}
