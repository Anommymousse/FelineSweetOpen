using System.Collections;
using System.Collections.Generic;
using DarkTonic.MasterAudio;
using MoreMountains.Feedbacks;
using Ricimi;
using UnityEngine;
using UnityEngine.UI;

public class BackButtonPress : MonoBehaviour
{
    public SceneTransition _SceneTransistion;
    Button _buttonToPress;
    public MMFeedbacks _mmFeedbacks;


    public void ButtonPressDoScaleThenCallOtherButton()
    {
        if (_SceneTransistion == null)
        {
            _SceneTransistion = GetComponent<SceneTransition>();
            if (_SceneTransistion == null)
            {
                Debug.Log("Scene can't be found");
            }
        }

        if (_buttonToPress == null)
        {
            _buttonToPress = GetComponent<Button>();
            if (_buttonToPress == null)
            {
                Debug.Log($" back button not found");
                return;
            }
        }

        MasterAudio.PlaySound("Button");
        
        _mmFeedbacks?.PlayFeedbacks();
        StartCoroutine(PressButtonAfterTimer());
    }

    IEnumerator PressButtonAfterTimer()
    {
        var waittime = _SceneTransistion.duration;
        yield return new WaitForSeconds(2.0f);
        _SceneTransistion.PerformTransition();
    }
}
