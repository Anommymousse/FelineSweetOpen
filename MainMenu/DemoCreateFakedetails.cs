using System;
using UnityEngine;

namespace MainMenu
{
    public class DemoCreateFakedetails : MonoBehaviour
    {
        void OnDestroy()
        {
            if (GameObject.Find("DifficultyObject") == null)
            {
                GameObject thing = new GameObject();
                thing.name = "DifficultyObject";
                thing.AddComponent<DifficultyLevel>();
                thing.GetComponent<DifficultyLevel>().SetDifficulty("Easy");
                DontDestroyOnLoad(thing);
            }

            if (GameObject.Find("CATID") == null)
            {
                GameObject thing = new GameObject();
                thing.name = "CATID";
                thing.AddComponent<DontDestroyCATID>();
                thing.GetComponent<DontDestroyCATID>().SetCatID(0);
                DontDestroyOnLoad(thing);
            }
            
           
        }
    }
}
