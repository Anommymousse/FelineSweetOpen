using System.Collections;
using System.Collections.Generic;
using DarkTonic.PoolBoss;
using UnityEngine;

public class AddToTimerAmount : MonoBehaviour
{
    [SerializeField] int timerAmountToAdd=100;
    void OnTriggerEnter2D(Collider2D other)
    {
        var player = other.GetComponent<Player>();
        if (player != null)
        {
            InGameCountDownTimer.AddToTimer(timerAmountToAdd);
            PoolBoss.Despawn(transform);
        }
    }
}