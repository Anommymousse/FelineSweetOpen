using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyCloudSkullMovement : MonoBehaviour
{
    GameObject playerRef;
    Tilemap nonHiddenMap;
    float _TimeBetweenMoves = 1.75f;
    float _timeToMove = 1.25f;
    Vector3Int _currentSkullcellposition;
    float _sizex;
    float _sizey;

    void OnEnable()
    {
        nonHiddenMap = GameObject.Find("NonHidden").GetComponent<Tilemap>();
        playerRef = GameObject.Find("Player");
        _currentSkullcellposition = nonHiddenMap.WorldToCell(transform.position);
        _sizex = nonHiddenMap.cellSize.x;
        _sizey = nonHiddenMap.cellSize.y;
    }

    void MoveTowardsPlayer()
    {

        var currentplayerPosition = playerRef.transform.position;
        var currentSkullPosition = transform.position;
        
        var worldDiff = currentplayerPosition - currentSkullPosition;
        var destposition = currentSkullPosition;
        
        if((Mathf.Abs(worldDiff.x))>Mathf.Abs((worldDiff.y)))
        {
            if (worldDiff.x < 0)
                destposition.x -= _sizex;
            if (worldDiff.x > 0)
                destposition.x += _sizex;
        }
        else
        {
            if (worldDiff.y < 0)
                destposition.y -= _sizey;
            if (worldDiff.y > 0)
                destposition.y += _sizey;
        }

        transform.DOMove(destposition, 0.8f);
    }

    void Update()
    {
        _timeToMove -= Time.deltaTime;
        if (_timeToMove < 0.0f)
        {
            MoveTowardsPlayer();
            _timeToMove = _TimeBetweenMoves;
        }
    }
}
