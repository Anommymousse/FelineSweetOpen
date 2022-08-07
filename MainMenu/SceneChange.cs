using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using MoreMountains.FeedbacksForThirdParty;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    [SerializeField] string SceneToGoTo="MainMenu";
    public void LoadInNewScene()
    {
        Debug.Log($" scene load = {SceneToGoTo}");
        StartCoroutine(FadeOutThenLoadScene());
    }

    IEnumerator FadeOutThenLoadScene()
    {
        FadeController.FadeOut(SceneToGoTo);
        yield return null;
        //SceneManager.LoadScene(SceneToGoTo);
    }
}
