using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackdropControlColor : MonoBehaviour
{
    public Image backdropA;
    public Image backdropB;
    public Image backdropC;
    public Image backdropD;
    public Image backdropE;
    public int cakemax = 321;
    
    // Start is called before the first frame update
    void Start()
    {
        //Grab cakes.
        //0 - cakemax
        
        int collectedcakes = KittyFund.GetCakeMoney();
        float percentage = (float)collectedcakes / (float)cakemax;
        
        if (percentage > 1.0f) percentage = 1.0f;
        if (percentage < 0.0f) percentage = 0.0f;

        percentage /= 2.0f;
        percentage += 0.5f;

        Color greyness = new Color(percentage, percentage, percentage, 1.0f); 

        backdropA.color = greyness;
        backdropB.color = greyness;
        backdropC.color = greyness;
        backdropD.color = greyness;
        backdropE.color = greyness;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
