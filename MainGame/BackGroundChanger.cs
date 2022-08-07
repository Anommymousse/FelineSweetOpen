using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using static MyExtensions.MyExtensions;

public class BackGroundChanger : MonoBehaviour
{
    SpriteRenderer _currentBackDrop;

    List<string> listOfBackDrops = new List<string>();
    string secretBD = "Space";
    Player _playerRef;

    void CreateListOfBackDrops()
    {
        if(listOfBackDrops.Count>0)
            listOfBackDrops.Clear();
        listOfBackDrops.Add("Ocean");
        listOfBackDrops.Add("Forest01");
        listOfBackDrops.Add("Bridge01");
        listOfBackDrops.Add("Forest02");
        listOfBackDrops.Add("Bridge02");
        listOfBackDrops.Add("Graveyard");
        listOfBackDrops.Add("Landscape01");
        listOfBackDrops.Add("Landscape02");
        listOfBackDrops.Add("Landscape03");
        listOfBackDrops.Add("Landscape04");
        listOfBackDrops.Add("Castle02");
        listOfBackDrops.Add("Landscape05");
        listOfBackDrops.Add("Landscape06");
        listOfBackDrops.Add("Landscape07");
        listOfBackDrops.Add("Landscape08");
        listOfBackDrops.Add("Landscape09");
        listOfBackDrops.Add("Landscape10");
        listOfBackDrops.Add("Castle01");
        listOfBackDrops.Add("Castle03");
        listOfBackDrops.Add("Castle04");
        listOfBackDrops.Add("Castle05");
        listOfBackDrops.Add("Castle06");
        listOfBackDrops.Add("Castle07");
        listOfBackDrops.Add("Town01");
        listOfBackDrops.Add("Town02");
        listOfBackDrops.Add("ChessRoom");
    }

    // Start is called before the first frame update
    void Start()
    {
        var go = GameObject.Find("Square");
        _currentBackDrop = go.GetComponent<SpriteRenderer>();
        Addressables.LoadAssetAsync<Sprite>("Ocean").Completed += OnLoadingCompleted;
    }

    void OnEnable()
    {
        CreateListOfBackDrops();        
        var go = GameObject.Find("Square");
        _currentBackDrop = go.GetComponent<SpriteRenderer>();

        ChangeToNextBackDrop();
       
        var wht = GameObject.Find("Player");
        _playerRef = wht.GetComponent<Player>();
        _playerRef.OnPlayerLevelChange += SignalBackDropCheck;
    }

    void OnDisable()
    {
        _playerRef.OnPlayerLevelChange -= SignalBackDropCheck;
    }

    void ChangeToNextBackDrop()
    {
        var GO = GameObject.Find("LevelManager");
        var levelloader = GO.GetComponent<LevelLoader>();
        var level = Int32.Parse(levelloader.GetLevelLevel());

        //int currentBD = level;
        int currentBD = level / 7;
        if (currentBD >= listOfBackDrops.Count-1) currentBD = listOfBackDrops.Count-1;

        Log($"Backdrop change to index {currentBD} {listOfBackDrops[currentBD]} ","red");

        Addressables.LoadAssetAsync<Sprite>(listOfBackDrops[currentBD]).Completed += OnLoadingCompleted;
    }

    void SignalBackDropCheck()
    {
        ChangeToNextBackDrop();
    }

    void OnLoadingCompleted(AsyncOperationHandle<Sprite> obj)
    {
        if (obj.Result)
        {
            _currentBackDrop.sprite = obj.Result;
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
