using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PingPongLock : MonoBehaviour
{
    float ZLockRotationMin = -20;
    float ZLockRotationMax = 20;
    float direction = 1f;
    float currentLockRotation;
    float speed = 25.0f;
    
    // Start is called before the first frame update
    void Start()
    {
        currentLockRotation = Random.Range(ZLockRotationMin, ZLockRotationMax);
        var lockdir = UnityEngine.Random.Range(0, 2);
        if (lockdir == 1)
            direction *= -1;
    }

    // Not ideal but only 10 of them
    void Update()
    {
        HandlePingPong();
        
    }

    void HandlePingPong()
    {
        currentLockRotation += speed * direction * Time.deltaTime;

        if (currentLockRotation < ZLockRotationMin)
        {
            currentLockRotation = ZLockRotationMin;
            direction *= -1.0f;
        }

        if (currentLockRotation > ZLockRotationMax)
        {
            currentLockRotation = ZLockRotationMax;
            direction *= -1.0f;
        }
        
        transform.localRotation = Quaternion.Euler(0,0,currentLockRotation); 
        
    }
}
