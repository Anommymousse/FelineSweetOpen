using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditorDetectInButton : MonoBehaviour
{
    public ManualCursorMouseAndGamepad _refCursor;    
    Button _refButton;
    Vector3[] _coords;
    EditorReplaceMouseCursor refToMouseCursor;
    float minx, miny, maxx, maxy;
    
    
    // Start is called before the first frame update corners
    void Start()
    {
        _refButton = GetComponent<Button>();
        _coords = new Vector3[4];
        _refButton.GetComponent<RectTransform>().GetWorldCorners(_coords);
        
        refToMouseCursor = GameObject.Find("[MouseStuffs]").GetComponent<EditorReplaceMouseCursor>();
        
        for (int i = 0; i < 4; i++)
        {
            _coords[i] = Camera.main.WorldToScreenPoint(_coords[i]);
        }

        minx = _coords[0].x;
        maxx = _coords[0].x;
        miny = _coords[0].y;
        maxy = _coords[0].y;
        for (int i = 1; i < 4; i++)
        {
            if (_coords[i].x < minx) minx = _coords[i].x;
            if (_coords[i].y < miny) miny = _coords[i].y;
            if (_coords[i].x > maxx) maxx = _coords[i].x;
            if (_coords[i].y > maxy) maxy = _coords[i].y;
        }
        
        //Debug.Log($"({minx},{miny})->({maxx},{maxy}) <color=red> XXXX </color> ");

        _refCursor = GameObject.Find("Cursor").GetComponent<ManualCursorMouseAndGamepad>();
    }

    bool IsInButtonWindow(float x,float y)
    {
        if((x>minx)&&(x<maxx))
        {
            if ((y > miny) && (y < maxy))
            {
                return true;
            }
        }
        return false;
    }    

    // Update is called once per frame
    void Update()
    {
        var cursorPosition = _refCursor.GetManualCursorCoords();

        if (IsInButtonWindow(cursorPosition.x, cursorPosition.y))
        {
            Debug.Log($"<color=green> go = {gameObject.name} </color>");
            var parentTransform1 = gameObject.transform.parent;         //enemy->image
            var parentTransform2 = parentTransform1.transform.parent;   //Image->pump

            var spriteName = parentTransform2.FindDeepChild("SpriteToReplace").GetComponent<SpriteRenderer>().sprite.name;
            
            
            refToMouseCursor.SetnewMouseCursor(spriteName);
            

                var parentTransform3 = parentTransform2.transform.parent;   //pump->button
                var parentTransform4 = parentTransform3.transform.parent;   //button->gameobject
                var parentTransform5 = parentTransform4.transform.parent;   //gameobj->4waybutton
                
                parentTransform5.gameObject.SetActive(false);
                

        }
    }
}
