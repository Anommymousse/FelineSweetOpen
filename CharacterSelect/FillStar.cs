using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FillStar : MonoBehaviour
{
    [SerializeField] Sprite _FilledStar;
    [SerializeField] int catID;
    //bool _isFilled;
    
    // Start is called before the first frame update
    void Start()
    {
        string key = catID + "Unlocked";
        int unlocked = PlayerPrefs.GetInt(key, 0);
        if (unlocked == 1)
        {
            FillThisStar();
        }
    }

    public void FillThisStar()
    {
        var spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = _FilledStar;
      //  _isFilled = true;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
