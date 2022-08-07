using System;
using System.Collections;
using System.Collections.Generic;
using DarkTonic.MasterAudio;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DemoEnd : MonoBehaviour
{
    //bool _perfectRun;
    int _respawnsUsed;
    [SerializeField] TMP_Text RespawnShadowText;
    [SerializeField] TMP_Text RespawnText;
    [SerializeField] ParticleSystem LeftParticleSystem;
    [SerializeField] ParticleSystem RightParticleSystem;
    [SerializeField] ParticleSystem particleSystem03;
    [SerializeField] ParticleSystem particleSystem04;
    void OnEnable()
    {
        _respawnsUsed = PlayerPrefs.GetInt("PlayerRespawnCount", 999);
        AssignRespawnCountToTexts();

        if (DifficultyLevel.IsCustomMode())
        {
            MasterAudio.PlaySound("EndOfCustomMode");
            return;
        }
        
        if (_respawnsUsed == 0)
        {
            if (!DifficultyLevel.IsCustomMode())
                GenericUnlockAchievement.UnlockAchievement("Purrfection");
            
            if (!DifficultyLevel.IsCustomMode())
                if(KittyFund.GotAllCakes()==false)
                    MasterAudio.PlaySound("AlphaVersionNoLivesLost");

            if (!DifficultyLevel.IsCustomMode())            
                PerfectRunExtras();    
        }
        else
        {
            //MasterAudio.PlaySound("DemoEndVoice");
            if (!DifficultyLevel.IsCustomMode())
                MasterAudio.PlaySound("AlphaVersionLivesLost");
            //_perfectRun = false;
        }

        if (!DifficultyLevel.IsCustomMode())
            if (KittyFund.GotAllCakes())
            {
                GenericUnlockAchievement.UnlockAchievement("CakeAgeddon");
                MasterAudio.PlaySound("ProperEnding");
                //Switch to proper end scene!
                RunFullEnding();
            }
    }

    void AssignRespawnCountToTexts()
    {
        RespawnShadowText.SetText(_respawnsUsed.ToString());
        RespawnText.SetText(_respawnsUsed.ToString());
    }

    void PerfectRunExtras()
    {
        RespawnShadowText.SetText(" 0 ");
        RespawnText.SetText("<rainb> 0 </rainb>");
        LeftParticleSystem.Play();
        RightParticleSystem.Play();
        particleSystem03.Play();
        particleSystem04.Play();
    }

    void RunFullEnding()
    {
        SceneManager.LoadScene("ProperEnding");
    }

    // Update is called once per frame
    void Update()
    {
    }
}
