using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorDisplay4WayButton : MonoBehaviour
{
    public ManualCursorMouseAndGamepad _refToCursor;
    public GameObject _fourWayReference;
    public EditorEnemyListGetNewEnemy _listToDisplay;
    bool _IsButtonActive;

    void Log(string thing)
    {
        Debug.Log($"{thing}");
    }
    
    // Start is called before the first frame update
    void Start()
    {
        Log("Start");
        if (_fourWayReference == null) return;
        Log("FourwayRefOK");
        _IsButtonActive = false;
    }

    void OnEnable()
    {
        Log("Enabled");
    }

    public void ActivateFourWay()
    {
        Log("Activate four way");
        _fourWayReference.SetActive(true);
        _IsButtonActive = true;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (_IsButtonActive==false) return;
        
        Log("<color=red>List and start ok</color>");

        //Check for continued mouse down.
        if (_refToCursor.IsCursorActivated())
        {
            Log("<color=red>Cursor Down</color>");
        }
        else
        {
            Log("<color=green>Let Go</color>");
            StartCoroutine(TurnOffAfterALittleBit());
            _IsButtonActive = false;
        }
    }

    IEnumerator TurnOffAfterALittleBit()
    {
        yield return new WaitForSeconds(1.0f);
        Log("<color=blue>Game object set to false</color>");
        _fourWayReference.SetActive(false);
        ///_fourWayReference.GetComponent<EditorSetActiveFalse>().TurnOffDamnYou();
    }
    
}
