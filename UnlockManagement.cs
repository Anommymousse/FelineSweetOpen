using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockManagement : MonoBehaviour
{
    public class UnlockKittens : MonoBehaviour
    {
        public void UnlockKitten(int catID)
        {
            string key = catID + "KittenID";
            PlayerPrefs.SetInt(key, 1);
        }
    }
}
