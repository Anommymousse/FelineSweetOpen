using System.Collections;
using System.Collections.Generic;
using DarkTonic.MasterAudio;
using DarkTonic.PoolBoss;
using UnityEngine;

public class TimedDepawn : MonoBehaviour
{
    public float TimeToDespawn = 3.5f;
    bool isDying = false;
    // Start is called before the first frame update
    void Start()
    {
        isDying = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(isDying==false)
            StartCoroutine(TimeTillDead());
    }

    IEnumerator TimeTillDead()
    {
        isDying = true;
        MasterAudio.PlaySound("BombBOOM");
        yield return new WaitForSeconds(TimeToDespawn);
        isDying = false;
        PoolBoss.Despawn(transform);
    }
}
