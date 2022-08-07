using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinueCreateObjectsForScanning : MonoBehaviour
{
    string _difficultyPPID = "DifficultyPPID";
    string _catIDPPID = "CatIDPPID";
    public void CreateDiffAndCatIDObjects()
    {
        GameObject diffObj = new GameObject();
        diffObj.name = "DifficultyObject";
        var thing = diffObj.AddComponent<DifficultyLevel>();
        thing.difficultySelected = PlayerPrefs.GetString(_difficultyPPID, "Normal");

        GameObject catObj = new GameObject();
        catObj.name = "CATID";
        var catid = catObj.AddComponent<DontDestroyCATID>();
        var catIDfromPlayerprefs = PlayerPrefs.GetInt(_catIDPPID, 0);
        //var catidObj = GameObject.Find("CATID").GetComponent<DontDestroyCATID>();
        catid.SetCatID(catIDfromPlayerprefs);
        
    }
 //CreateCatId   
}
