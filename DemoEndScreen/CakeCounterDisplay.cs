using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CakeCounterDisplay : MonoBehaviour
{
    public TMP_Text cakeText;
    public TMP_Text cakeText2;
    // Start is called before the first frame update
    void Start()
    {
        int cakes = KittyFund.GetCakeMoney();
        int cakemax = KittyFund.GetCakesMax();
        
        cakeText.SetText($"{cakes} of {cakemax}");
        cakeText2.SetText($"{cakes} of {cakemax}");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
