using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DarkTonic.MasterAudio;
using DG.Tweening;
using HeathenEngineering.SteamAPI;
using MoreMountains.Feedbacks;
using UnityEngine;

public class EditorBunnyButtonPress : MonoBehaviour
{
    bool _isActive = false;
    [SerializeField] SpriteRenderer rightPaw;
    [SerializeField] SpriteRenderer leftPaw;
    [SerializeField] SpriteRenderer bunnyEyesClosed;
    [SerializeField] SpriteRenderer bunnyEyesOpen;
    [SerializeField] GameObject leftEar;
    [SerializeField] GameObject rightEar;
    [SerializeField] MMFeedbacks cameraShake;

    public void BunnyButtonActivate()
    {
        if (_isActive == false)
        {
            _isActive = true;
            GenericUnlockAchievement.UnlockAchievement("BunnyMeme");
            StartCoroutine(BunnyEarsAnimate());
        }
        
    }

    IEnumerator BunnyEarsAnimate()
    {
        Vector3 start = Vector3.one*30.0f;
        start.z = 1.0f;
        Vector3 end = Vector3.one*24.0f;
        end.z = 1.0f;

        yield return null;
        bunnyEyesOpen.color = Color.white;
        rightPaw.color = Color.white;
        leftPaw.color = Color.white;
        yield return new WaitForSeconds(0.01f);
        bunnyEyesClosed.color = Color.clear;

        float newy = -345.0f;
        float newytostart = -360.4f;
        rightPaw.transform.DOLocalMoveY(newy,0.4f);
        leftPaw.transform.DOLocalMoveY(newy,0.4f);
        leftEar.transform.DOScale(start,0.4f);
        rightEar.transform.DOScale(start,0.4f);
        yield return new WaitForSeconds(0.4f);
        leftEar.transform.DOScale(end,0.1f);
        rightEar.transform.DOScale(end, 0.1f);
        rightPaw.transform.DOLocalMoveY(newytostart, 0.1f);
        leftPaw.transform.DOLocalMoveY(newytostart, 0.1f);
        cameraShake.PlayFeedbacks();
        MasterAudio.PlaySound("FeelBounceLanding");
        yield return new WaitForSeconds(0.4f);

        GenericUnlockAchievement.UnlockAchievement("BunnyMeme");

        bunnyEyesClosed.color = Color.white;
        rightPaw.color = Color.clear;
        leftPaw.color = Color.clear;
        bunnyEyesOpen.color = Color.clear;
        
        _isActive = false;
    }
    
    

    // Start is called before the first frame update
    void Start()
    {
        _isActive = false;
    }

}
