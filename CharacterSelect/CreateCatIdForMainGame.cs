using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateCatIdForMainGame : MonoBehaviour
{
    [SerializeField] int catId;

    public void SetCatIDObject()
    {
        DontDestroyCATID go = GameObject.Find("CATID").GetComponent<DontDestroyCATID>();
        go.SetCatID(catId);
        
        PlayerPrefs.SetInt("CatIDPPID", catId);
    }
}
