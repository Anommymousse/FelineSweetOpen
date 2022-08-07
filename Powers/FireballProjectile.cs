using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class FireballProjectile : MonoBehaviour
{
    float Xspeed;
    float Xdirection;
    Vector3 offset=Vector3.zero;

    public void SetDirection(float direction) => Xdirection = direction;
    public void SetSpeed(float speed) => Xspeed = speed;


    public void AdjustOffset(Vector3 newOffset) => transform.position += newOffset;

    // Update is called once per frame
    void Update()
    {
        var amount = gameObject.transform.position.x;
        amount += Xspeed * Xdirection;
        gameObject.transform.DOMoveX(amount, 0.1f);
    }
}
