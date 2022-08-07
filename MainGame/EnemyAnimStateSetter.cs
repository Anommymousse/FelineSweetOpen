using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening.Core;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyAnimStateSetter : BasicEnemy
{
    
    public float speed = 4.0f;
    public float raysize=3.7f;
    public float set_xdirection = 1.0f;
    float _xdirection;

    // Start is called before the first frame update
    protected override void OnEnable()
    {
        base.OnEnable();
        _xdirection = set_xdirection;
        if (_xdirection < 0f)
        {
            //Penguin and magma walk
            if(base._direction > 0)
                base._direction = -1.0f;

            var sr = gameObject.GetComponentInChildren<SpriteRenderer>();
            if (sr)
            {
                sr.flipX = false;
            }
        }
        else
        {
            var sr = gameObject.GetComponentInChildren<SpriteRenderer>();
            if (sr)
                sr.flipX = true;
        }
    }
    
    void HandleAnimState()
    {
        if (_animatorBase == null) return;
        
        var thing2 = gameObject.transform.position;
        var rchit = Physics2D.RaycastAll(thing2, Vector2.down, raysize);

        
        if (rchit.Length>0)
        {
            SetIsGrounded(true); 
            base._animatorBase.SetBool("isGrounded",true);
        }
        else
        {
            base._animatorBase.SetBool("isGrounded",false);
            SetIsGrounded(false);
        }
        
        //Update is walking.
        if (Mathf.Abs(base._rigidbody2D.velocity.x) > float.Epsilon)
        {
            SetIsWalking(true);
            if (_animatorBase.GetBool("isWalking") == false)
            {
                base._animatorBase.SetBool("isWalking", true);
  //              Debug.Log("Walk Set");
            }
        }
        else
        {
            SetIsWalking(false);
            base._animatorBase.SetBool("isWalking",false);
//            Debug.Log("Walk unset");
        }
    }

    new void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log($"{other.collider.name}");
        Vector2 vector2direction = Vector2.zero;
        
        var playercheck = other.collider.GetComponent<Player>();
        if (playercheck != null)
        {
            playercheck.KillThePlayer(gameObject.name);
            return;
        }

        var thing = other.GetContact(0);
        var contactpoint = BrickMap.NonHiddenTilemap.WorldToCell(thing.point);
        
        if (BrickMap.NonHiddenTilemap.HasTile(contactpoint))
        {
            var enemyPoint = BrickMap.NonHiddenTilemap.WorldToCell(transform.position);
            
            if ((contactpoint.x == enemyPoint.x) && ((contactpoint.y + 1) == enemyPoint.y)) return;
            
            var tilename = BrickMap.NonHiddenTilemap.GetTile<Tile>(contactpoint);
            Debug.Log($"enemy tile ?{tilename} {thing.point}");
            BrickMap.DestroyBrick(contactpoint);
        }    
    }
    
    // Update is called once per frame
    void Update()
    {
        HandleAnimState();
        OncePerFrameMovement();
    }
    
}
