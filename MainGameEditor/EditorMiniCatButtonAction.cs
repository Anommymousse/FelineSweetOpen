using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DarkTonic.MasterAudio;
using DG.Tweening;
using HeathenEngineering.SteamAPI;
using MoreMountains.Feedbacks;
using UnityEngine;

public class EditorMiniCatButtonAction : MonoBehaviour
{
    bool _isActive;
    [SerializeField] MMFeedbacks _mmFeedbackActions;
    [SerializeField] SpriteRenderer eyesOpenSprite;

    public void ButtonActivation()
    {
        if (_isActive == false)
        {
            _isActive = true;
            Debug.Log($"Activate the kitty");   
            
            GenericUnlockAchievement.UnlockAchievement("PetTheKitty");
            
            StartCoroutine(LilCatButtonAction());
        }
    }

    IEnumerator LilCatButtonAction()
    {
        yield return null;
        eyesOpenSprite.color = Color.white;
        _mmFeedbackActions.PlayFeedbacks();
        MasterAudio.PlaySound("Cat_Purr_01");
        yield return new WaitForSeconds(0.1f);
        eyesOpenSprite.color = Color.clear;
        yield return new WaitForSeconds(0.2f);
        eyesOpenSprite.color = Color.white;
        yield return new WaitForSeconds(0.1f);
        eyesOpenSprite.color = Color.clear;
        yield return new WaitForSeconds(0.1f);
        eyesOpenSprite.color = Color.white;
        yield return new WaitForSeconds(4.0f);
        eyesOpenSprite.color = Color.clear;

        _isActive = false;
        
        GenericUnlockAchievement.UnlockAchievement("PetTheKitty");

    }
    

    // Start is called before the first frame update
    void Start()
    {
        _isActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
