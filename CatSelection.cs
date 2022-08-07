using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class CatSelection : MonoBehaviour
{
    int _currentId = 0;
    [SerializeField] List<RuntimeAnimatorController> _cats;

    void Awake()
    {
        var gothing = GameObject.Find("CATID");
        if (gothing!=null)
        {
            DontDestroyCATID go = gothing.GetComponent<DontDestroyCATID>();
            SetCat(go.GetCatID());
        }
    }

    public bool SetCat(int catId)
    {
        var temp = GetComponent<Animator>();
        
        Debug.Log($" trying to set catid = {catId}");
        
        int maxId = _cats.Count - 1;
        
        Debug.Log($" max is = {maxId}");
        if (catId > maxId) return false;
        
        Debug.Log($"Worked Setting to {catId}");
        
        temp.runtimeAnimatorController = _cats[catId];
        return true;
    }
    
    
    public void TestInc()
    {
        _currentId++;
        if (SetCat(_currentId) == false)
        {
            _currentId = 0;
            SetCat(_currentId);
        }
    }
    
}
