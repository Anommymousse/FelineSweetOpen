using System;
using System.Collections;
using System.Collections.Generic;
using Ricimi;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class LevelSelect : MonoBehaviour
{
    [SerializeField] int TotalLevels;
    public GameObject prefabLevelUnlocked; 
    public GameObject prefabLevelLocked;
    public int LevelPages = 3;
    
    int _currentLevelPage; //10 per page
    int _amountPerPage = 10;
    //int _mode = 0; //0:Easy, 1 medium, 2 hard.
    int _currentLevelCleared = 0; //Needs to be a playerprefs thing
    List<bool> _inactiveOrActive; //Note need to be able to play the next level :P
    List<Vector2> PositionList;
    
    List<Vector2> ButtonPositionList;
    GameObject originalButtonsGameObject;

    List<GameObject> activeList;

    static string DifficultyLevel = "Easy";

    public int GetCurrentLevelPage() => _currentLevelPage;

    void SetupButtonPositions()
    {
        PositionList = new List<Vector2>();
        ButtonPositionList = new List<Vector2>();

        TotalLevels = 100;
        
        PositionList.Add(new Vector2(-659f,193f));
        PositionList.Add(new Vector2(-295f,193f));
        PositionList.Add(new Vector2(  72f,193f));
        PositionList.Add(new Vector2( 427f,193f));
        PositionList.Add(new Vector2( 788f,193f));
        PositionList.Add(new Vector2(-652f,-149f));
        PositionList.Add(new Vector2(-295f,-149f));
        PositionList.Add(new Vector2(  72f,-149f));
        PositionList.Add(new Vector2( 427f,-149f));
        PositionList.Add(new Vector2( 788f,-149f));
    }

    public void CreateButtons()
    {
        var go = GameObject.Find("Levels");

        foreach (var thing in activeList)
        {
            Destroy(thing);
        }
        activeList.Clear();
        
        for (int i = 0; i < _amountPerPage; i++)
        {
            if (_inactiveOrActive[i])
            {
                CreateButtonInstanceUnlocked(go,i);
            }
            else
            {
                CreateButtonInstanceLocked(go,i);
            }
        }
    }

    void CreateButtonInstanceLocked(GameObject go,int i)
    {
        var buttonobject = Instantiate(prefabLevelLocked, go.transform );
        activeList.Add(buttonobject);
        //Set local position.
        buttonobject.transform.localPosition = new Vector3(PositionList[i].x,PositionList[i].y,0f);
        buttonobject.SetActive(true);
        buttonobject.GetComponent<PopupOpener>().popupLevel = i+1+_currentLevelPage*_amountPerPage;
    }

    void CreateButtonInstanceUnlocked(GameObject go,int i)
    {
        if (i > 0)
        {
            Debug.Log($"Stopppy");
        }
        var buttonobject = Instantiate(prefabLevelUnlocked, go.transform );
        activeList.Add(buttonobject);
        buttonobject.transform.localPosition = new Vector3(PositionList[i].x,PositionList[i].y,0f);
        
        var popupLevelOpener = buttonobject.GetComponent<PlayPopupOpener>();
        popupLevelOpener.SetLevel(i+1+_currentLevelPage*_amountPerPage);
        popupLevelOpener.level = i+1+_currentLevelPage*_amountPerPage;
        
        var textToSet = buttonobject.GetComponentInChildren<Text>();
        textToSet.text = $"{i+1+_currentLevelPage*_amountPerPage}";
        
        ButtonSetStars(buttonobject);
        
        buttonobject.SetActive(true);
        
        //var thing = buttonobject.GetComponent<PlayPopup>();
        //thing.LevelTextSet(i + 1 + _currentLevelPage * _amountPerPage);
    }

    void Awake()
    {
        ReadDifficultyLevel();
        SetMaxLevelsBasedOnDifficulty();
        _currentLevelCleared = ES3.Load("UsernameLevel"+DifficultyLevel, 0);
        _currentLevelPage = _currentLevelCleared / _amountPerPage;
        LevelPages = TotalLevels / _amountPerPage;  
        
        activeList = new List<GameObject>();
        SetupButtonPositions();
        SetupButtonActiveOrInactive(_currentLevelPage);
        CreateButtons();
        
        Debug.Log("Done?");
    }

    void SetMaxLevelsBasedOnDifficulty()
    {
        if (DifficultyLevel == "Master") TotalLevels = 50;
        if (DifficultyLevel == "Hard") TotalLevels = 50;
        if (DifficultyLevel == "Medium") TotalLevels = 50;
        if (DifficultyLevel == "Easy") TotalLevels = 30;
    }

    static void ReadDifficultyLevel()
    {
        var diffObj = GameObject.Find("DifficultyObject");
        if (diffObj != null)
        {
            DifficultyLevel = diffObj.GetComponent<DifficultyLevel>().GetDifficultyNS();
        }
    }

    public void SetupButtonActiveOrInactive(int levelPage=0)
    {
        _inactiveOrActive = new List<bool>();
        int startpage = levelPage;
        _currentLevelPage = levelPage;
        
        int activeCountThisPage = _currentLevelCleared - (startpage * _amountPerPage) + 1;
        for (int i = 0; i < _amountPerPage; i++)
        {
            _inactiveOrActive.Add(activeCountThisPage > 0);
            activeCountThisPage--;
        }
    }

    

    void ButtonSetStars(GameObject buttonobject)
    {
        var popupOpener = buttonobject.GetComponent<PlayPopupOpener>();
        
        int starsRnd = Random.Range(0, 4);
        
        popupOpener.starsObtained = GetStarsForLevel(DifficultyLevel,popupOpener.level);

        int starsCollectedforLevel = popupOpener.starsObtained;
        int maxgoldstarsforlevel = LevelItemCounter.GetItemCountMaxForLevel(DifficultyLevel,popupOpener.level.ToString(),"N","Gold_Star");
        
        //Ok, smol plan
        //Set grey for max stars.
        //Set gold for starscollected
        

        var parent = buttonobject.transform.Find("Parent");

        var star1 = parent.transform.Find("Star 1");
        var star2 = parent.transform.Find("Star 2");
        var star3 = parent.transform.Find("Star 3");

        var bluestar1 = star1.Find("Blue Star");
        var Greystar1 = star1.Find("Grey Star");
        var Goldstar1 = star1.Find("Gold Star");
        var bluestar2 = star2.Find("Blue Star");
        var Greystar2 = star2.Find("Grey Star");
        var Goldstar2 = star2.Find("Gold Star");
        var bluestar3 = star3.Find("Blue Star");
        var Greystar3 = star3.Find("Grey Star");
        var Goldstar3 = star3.Find("Gold Star");
                
                
        bluestar1.gameObject.SetActive(false);
        bluestar2.gameObject.SetActive(false);
        bluestar3.gameObject.SetActive(false);
                
        Greystar1.gameObject.SetActive(maxgoldstarsforlevel>0);
        Greystar2.gameObject.SetActive(maxgoldstarsforlevel>1);
        Greystar3.gameObject.SetActive(maxgoldstarsforlevel>2);
        
        Goldstar1.gameObject.SetActive(starsCollectedforLevel>0);
        Goldstar2.gameObject.SetActive(starsCollectedforLevel>1);
        Goldstar3.gameObject.SetActive(starsCollectedforLevel>2);

    }

    int GetStarsForLevel(string difficulty, int level)
    {
        return LevelItemCounter.GetItemCountForLevel(difficulty, level.ToString(), "N", "Gold_Star");
    }

    void Update()
    {
        
    }
}
