using System;
using System.Collections;
using System.Collections.Generic;
using DarkTonic.MasterAudio;
using DarkTonic.PoolBoss;
using UnityEngine;

public class ShieldControl : MonoBehaviour
{
    static bool _isShieldActive;
    static bool _isShieldBeingDeactivated;
    public GameObject ShieldObject;

    bool _hasStartedLoopingSound;
    //Assumes attached to player as child

    void Start()
    {
        _isShieldActive = false;
        _hasStartedLoopingSound = false;
        ShieldObject.SetActive(false);
        _isShieldBeingDeactivated = false;
    }
    
     
    public static bool GetShieldState() => _isShieldActive;

    public void ActivateShield()
    {
        MasterAudio.PlaySound("magic_cast_generic03");
        MasterAudio.SetBusVolumeByName("ShieldLoop",1.0f);
        if (_hasStartedLoopingSound == false)
        {
            _hasStartedLoopingSound = true;
            var thing = MasterAudio.PlaySound("LightningLoop");
            
        }

        _isShieldActive = true;
        ShieldObject.SetActive(true);
    }

    public void DeactivateShield()
    {
        if(_isShieldActive)
            if(_isShieldBeingDeactivated==false)
                StartCoroutine(ShieldGoingDown());
    }

    IEnumerator ShieldGoingDown()
    {
        _isShieldBeingDeactivated = true;
        MasterAudio.PlaySound("stormimpact03");
        MasterAudio.SetBusVolumeByName("ShieldLoop",0.0f);
        yield return new WaitForSeconds(1.0f);
        _isShieldActive = false;
        _isShieldBeingDeactivated = false;
        ShieldObject.SetActive(false);
    }


    void OnCollisionEnter2D(Collision2D other)
    {
        if(_isShieldActive)
        if (other.collider.name != "NonHidden")
        {
            DeactivateShield();
            PoolBoss.Despawn(other.transform);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(_isShieldActive)
        if (other.name != "NonHidden")
        {
            DeactivateShield();
            PoolBoss.Despawn(other.transform);
        }
    }
}
