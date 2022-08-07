using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class PlayButton : MonoBehaviour
{
    public static string LevelToLoad;
    [SerializeField] string _levelName;
    void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() => LevelToLoad = _levelName);
    }
}
