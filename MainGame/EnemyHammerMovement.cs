using System;
using System.Collections;
using System.Collections.Generic;
using DarkTonic.PoolBoss;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.XR;

public class EnemyHammerMovement : MonoBehaviour
{
    SpriteRenderer _spriteRenderer;
    public AnimationCurve _animationXRotationCurve;
    public float MoveSpeed = 0.1f;
    bool ycorrectionHappened;
    BrickMap _brickMapRef;
    Transform _leftFeeler;
    Transform _rightFeeler;
    float xdirection = -1;
    RaycastHit2D[] _raycastHit2Ds;
    GameObject _parentBaseGameObject;

    // Start is called before the first frame update
    void Awake()
    {
        
        _spriteRenderer = GetComponent<SpriteRenderer>();
        
        //HAMMER WAIT?
        //WaitForPoolBossThenLoad();

        SequenceHammer();
        _parentBaseGameObject = transform.parent.gameObject;
        _leftFeeler = transform.parent.Find("LeftFeeler");
        _rightFeeler = transform.parent.Find("RightFeeler");
        _raycastHit2Ds = new RaycastHit2D[10];

        _brickMapRef = GameObject.Find("TilesBoss").GetComponent<BrickMap>();
    }
    
    void SequenceHammer()
    {
        var deltaPos = transform.position;
        deltaPos.y -= 0.5f;
        transform.DOMove(deltaPos, 0.01f, true);
        
        Sequence mySequence = DOTween.Sequence();
        mySequence.SetLoops(-1, LoopType.Yoyo);
        
        mySequence.Append(transform.DOLocalRotate(new Vector3(0,0,50), 0.5f));
        mySequence.Append(transform.DOLocalRotate(new Vector3(0,0,-50), 0.5f));
        mySequence.Append(transform.DOLocalRotate(new Vector3(0,0,40), 0.3f));
        mySequence.Append(transform.DOLocalRotate(new Vector3(0,0,-40), 0.3f));
    }
    
    //True on static
    bool CheckForCollisionSides(Vector3 ParentworldPosition)
    {
        Vector2 _leftOrRight= Vector2.left;
        if (xdirection > 0) _leftOrRight = Vector2.right;
        ParentworldPosition.y += 0.5f;
        
        int hits = Physics2D.RaycastNonAlloc(ParentworldPosition, _leftOrRight, _raycastHit2Ds, 0.8f, 1 << 12);
        for (int i = 0; i < hits; i++)
        {
            if (_raycastHit2Ds[i].collider.name == "NonHidden")
            {
                Vector3 worldPosition = ParentworldPosition;
                worldPosition.x += _leftOrRight.x * 0.3f;
                var cellpos = _brickMapRef.NonHiddenTilemap.WorldToCell(worldPosition);
//                Debug.Log($" wp = {worldPosition} cell pos = {cellpos} destruct = {_bricksRef.IsDestructibleTile(cellpos)}");
                _brickMapRef.DestroyBrick(worldPosition);
                
                var thing = _brickMapRef.NonHiddenTilemap.GetTile<Tile>(cellpos);
                if(thing!=null)
                    if (thing.name.Contains("static"))
                    {
                        return true;
                    }
            }
        }

        return false;
    }

    bool CheckForCollisions(Vector3 positionToCastFrom)
    {
        int hits = Physics2D.RaycastNonAlloc(positionToCastFrom, Vector2.down, _raycastHit2Ds,1.0f,1<<12 );
        
        for (int i = 0; i < hits; i++)
        {
  //          Debug.Log($"hits are {_raycastHit2Ds[i].collider.name}");
            if (_raycastHit2Ds[i].collider.name == "NonHidden") return true;
        }

        return false;
    }

    void HandleMovement()
    {
        
        //Debug.Log($"HM : {CheckForCollisions(_rightFeeler.position)}");
        //Debug.Log($"HM : {CheckForCollisions(_leftFeeler.position)}");
        //Debug.Log($"HM : {xdirection}");
        
    //    Debug.Log($"  world->cell {_bricksRef.NonHiddenTilemap.WorldToCell(_parentBaseGameObject.transform.position)}");

        //Debug.Log($"HM : wp {_parentBaseGameObject.transform.position}");
        if (CheckForCollisionSides(_parentBaseGameObject.transform.position))
        {
            xdirection *= -1;
        }
        
        if (xdirection > 0)
        {
            if (CheckForCollisions(_rightFeeler.position) == false)
            {
                xdirection = -1;
            }
        }
        else
        {
            if (xdirection < 0)
            {
                if (CheckForCollisions(_leftFeeler.position) == false)
                {
                    xdirection = 1;
                }
            }
        }
        
        Vector3 newposition = _parentBaseGameObject.transform.position;
        newposition.x += xdirection * MoveSpeed;

        if (ycorrectionHappened == false)
        {
            newposition.y -= 0.5f;
            ycorrectionHappened = true;
        }
        _parentBaseGameObject.transform.position = newposition;
        
    }

    void FixedUpdate()
    {
        HandleMovement();
    }

    
    // Update is called once per frame
    void Update()
    {
        
    }
}
