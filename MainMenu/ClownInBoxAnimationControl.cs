using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HeathenEngineering.SteamAPI;
using UnityEngine;

public class ClownInBoxAnimationControl : MonoBehaviour
{
    [SerializeField] AudioClip _PopUpAudio;
    [SerializeField] AudioClip _SideToSideAudio;
    [SerializeField] AudioClip _HideAudio;
    [SerializeField] AudioClip _MouseClickAudio;
    [SerializeField] ParticleSystem _particleSystem01;
    [SerializeField] ParticleSystem _particleSystem02;
    public AchievementObject _achievement;
    
    Animator anim;
    AudioSource _audioSource;
    readonly int _clownShouldPopUp = Animator.StringToHash("ClownShouldPopup");
    readonly int _clownShouldSideToSide = Animator.StringToHash("ClownShouldSideToSide");
    readonly int _clownShouldHide = Animator.StringToHash("ClownShouldHide");
    bool _UpdatingAnimation = false;
    int _animationStage = 0;
    int mouseClicks = 0;
    bool _buttonInactive=false;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        _animationStage = 0;
        mouseClicks = 0;
    }

    public void MouseClicked()
    {
        if(_buttonInactive==false)
        {
            mouseClicks++;
            if (mouseClicks > 1)
                UpdateAnimationStage();
            _audioSource.PlayOneShot(_MouseClickAudio);
            StartCoroutine(WaitForASecond());
        }
    }

    void UpdateAnimationStage()
    {
        if (_UpdatingAnimation == true) return;
        _UpdatingAnimation = true;
        _animationStage++;
        mouseClicks = 0;

        if (_animationStage == 1)
        {
            GenericUnlockAchievement.UnlockAchievement("ClickerGame");
            _audioSource.PlayOneShot(_PopUpAudio);
            anim.SetBool(_clownShouldPopUp, true);
            anim.SetBool(_clownShouldSideToSide,false);
            anim.SetBool(_clownShouldHide,false);
        }

        if (_animationStage == 2)
        {
            _particleSystem01.Play();
            _particleSystem02.Play();
            anim.SetBool(_clownShouldSideToSide, true);
        }

        if(_animationStage==3)
        {
            _particleSystem01.Stop();
            _particleSystem02.Stop();
            _audioSource.PlayOneShot(_HideAudio);
            anim.SetBool(_clownShouldHide,true);
            anim.SetBool(_clownShouldSideToSide,false);
            anim.SetBool(_clownShouldPopUp,false);
            _animationStage = 0;

            GenericUnlockAchievement.UnlockAchievement("ClickerGame");
        }
        
        _UpdatingAnimation = false;
    }
    

    IEnumerator WaitForASecond()
    {
        _buttonInactive = true;        
        yield return new WaitForSeconds(0.5f);
        _buttonInactive = false;
    }

}
