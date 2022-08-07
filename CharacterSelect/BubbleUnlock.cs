//#define DEMO
using System;
using System.Collections;
using System.Collections.Generic;
using DarkTonic.MasterAudio;
using UnityEngine;
using UnityEngine.EventSystems;
using static MyExtensions.MyExtensions;

public class BubbleUnlock : MonoBehaviour
{
    [SerializeField] GameObject bubbleGameObject;
    [SerializeField] GameObject chestGameObject;
    [SerializeField] ParticleSystem _particleSystem1;
    [SerializeField] ParticleSystem _particleSystem2;
    [SerializeField] ParticleSystem _particleSystem3;
    [SerializeField] int cost = 9;
    [SerializeField] int catID = 0;
    bool _unlocked;

    public int GetCatID() => catID;
    // Start is called before the first frame update
    void Start()
    {
        //Too lazy to use : ContextMenu since I want a clean state
        /*string key = catID + "Unlocked";
        PlayerPrefs.SetInt(key, 0);
        var theKittyFund = FindObjectOfType<KittyFund>();
        theKittyFund.SetMoney(catID, 30);*/
        
        //RELOCK ALL KITTYS
        //LockKitten(catID);
        
        _unlocked = IsKittenUnlocked(catID);
        if (_unlocked)
        {
            UnlockTheKitten(catID);
        }

        if (catID == 0)
        {
            if (_unlocked == false)
                UnlockTheKitten(0);//First timer
        }
    }

    void CheckFor3StarsUnlock()
    {
        if (IsKittenUnlocked(0) && IsKittenUnlocked(1) && IsKittenUnlocked(2))
        {
            var chestToDisable = FindObjectOfType<ChestEasy>();
            if(chestToDisable!=null)
                chestToDisable.gameObject.SetActive(false);
            if(IsKittenUnlocked(3)==false)
                UnlockTheKitten(3);
        }
        if (IsKittenUnlocked(4) && IsKittenUnlocked(5) && IsKittenUnlocked(6))
        {
            var chestToDisable = FindObjectOfType<ChestMedium>();
            if(chestToDisable!=null)
                chestToDisable.gameObject.SetActive(false);
            if(IsKittenUnlocked(7)==false)
                UnlockTheKitten(7);
            
        }
        if (IsKittenUnlocked(8) && IsKittenUnlocked(9) && IsKittenUnlocked(10))
        {
            var chestToDisable = FindObjectOfType<ChestHard>();
            if(chestToDisable!=null)
                chestToDisable.gameObject.SetActive(false);
            if(IsKittenUnlocked(11)==false)
                UnlockTheKitten(11);
        }
    }
    
    public void TryToUnlockTheKitten()
    {
        
        var theKittyFund = FindObjectOfType<KittyFund>();
        int currency = theKittyFund.GetMoney(catID);

        Log($"Buy with {catID} amt left{currency} {Time.time}","cyan");

        if (canKittenBeBought(currency))
        {
            Log($"Kittey can be bought");
            currency -= cost;
            theKittyFund.SetMoney(catID, currency);
            UnlockTheKitten(catID);
        }
        else
        {
            Log($"No kittey pets for you");
            MasterAudio.PlaySound("Glass_Tap_mono");
            //var audiosource = GetComponent<AudioSource>();
            //audiosource.PlayOneShot(failToUnlockSound);
        }
    }

    void StarAttained(int catId)
    {
        var starToFill = GetComponentInChildren<FillStar>();
        if(starToFill!=null)
            starToFill.FillThisStar();
        CheckFor3StarsUnlock();
    }

    bool canKittenBeBought(int currency)
    {
        bool canBuy = currency >= cost;
        return canBuy;
    }

    void LockKitten(int catIdenitity)
    {
        string key = catIdenitity + "Unlocked";
        PlayerPrefs.SetInt(key,0);
    }
    
    public bool IsKittenUnlocked(int catIdentity)
    {
        string key = catIdentity + "Unlocked";
        int isUnlocked = PlayerPrefs.GetInt(key);
        //Debug.Log($"{catIdentity} {isUnlocked}");
        if (isUnlocked == 1) return true;
        return false;
    }

    void UnlockTheKitten(int catIdentity)
    {
        string key = catIdentity + "Unlocked";
        PlayerPrefs.SetInt(key, 1);
        
        if (_particleSystem1 != null)
        {
            Instantiate(_particleSystem1, transform);
            _particleSystem1.Play();
        }

        if (_particleSystem2 != null)
        {
            Instantiate(_particleSystem2, transform);
            _particleSystem2.Play();
        }

        if (_particleSystem3 != null)
        {
            Instantiate(_particleSystem3, transform);
            _particleSystem3.Play();
        }

        if(bubbleGameObject!=null)
            bubbleGameObject.SetActive(false);

        MasterAudio.PlaySound("Button");
        //var audiosource = GetComponent<AudioSource>();
        //audiosource.PlayOneShot(UnlockSuccessSound);

        StarAttained(catID);

    }
}
