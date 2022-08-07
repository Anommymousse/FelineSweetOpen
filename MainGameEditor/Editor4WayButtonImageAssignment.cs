using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Editor4WayButtonImageAssignment : MonoBehaviour
{
    Transform _buttonLeft;
    Transform _buttonRight;
    Transform _buttonUp;
    Transform _buttonDown;
    
    
    // Start is called before the first frame update
    void OnEnable()
    {
        _buttonLeft = transform.FindDeepChild("ButtonLeft");
        _buttonRight = transform.FindDeepChild("ButtonRight");
        _buttonUp = transform.FindDeepChild("ButtonUp");
        _buttonDown = transform.FindDeepChild("ButtonDown");

        var list = GetComponent<EditorEnemyListGetNewEnemy>();

        List<SpriteRenderer> spriteToReplace = new List<SpriteRenderer>();
        List<Image> spriteEnemyImageReplace = new List<Image>();
        
        spriteToReplace.Add(_buttonLeft.FindDeepChild("SpriteToReplace").GetComponent<SpriteRenderer>());
        spriteToReplace.Add(_buttonRight.FindDeepChild("SpriteToReplace").GetComponent<SpriteRenderer>());
        spriteToReplace.Add(_buttonUp.FindDeepChild("SpriteToReplace").GetComponent<SpriteRenderer>());
        spriteToReplace.Add(_buttonDown.FindDeepChild("SpriteToReplace").GetComponent<SpriteRenderer>());
        
        spriteEnemyImageReplace.Add(_buttonLeft.FindDeepChild("Image").Find("Enemy").GetComponent<Image>());
        spriteEnemyImageReplace.Add(_buttonRight.FindDeepChild("Image").Find("Enemy").GetComponent<Image>());
        spriteEnemyImageReplace.Add(_buttonUp.FindDeepChild("Image").Find("Enemy").GetComponent<Image>());
        spriteEnemyImageReplace.Add(_buttonDown.FindDeepChild("Image").Find("Enemy").GetComponent<Image>());

        if (list == null)
        {
            Debug.Log("<color=blue>List is empty</color>");
            return;
        }
        
        if (list != null)
        {
            Debug.Log($"<color=red>list name = {list.gameObject.name} </color>");
            
            _buttonLeft.gameObject.SetActive(true);
            _buttonRight.gameObject.SetActive(false);
            _buttonUp.gameObject.SetActive(false);
            _buttonDown.gameObject.SetActive(false);
            
            if (list.EnemyList.Count > 1)
                _buttonRight.gameObject.SetActive(true);
            if(list.EnemyList.Count > 2)
                _buttonUp.gameObject.SetActive(true);
            if(list.EnemyList.Count > 3)
                _buttonDown.gameObject.SetActive(true);
        }

        var sprites = list.EnemyList.ToList();
        
        for (var i = 0; i < sprites.Count; i++)
        {
            Debug.Log($"Replacing sprite{i}");
            //Interesting if storing sprite array it won't work.
            spriteToReplace[i].sprite = sprites[i];
            spriteEnemyImageReplace[i].sprite = sprites[i];
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public static class TransformDeepChildExtension
{
    /*
    //Breadth-first search
    public static Transform FindDeepChild(this Transform aParent, string aName)
    {
        Queue<Transform> queue = new Queue<Transform>();
        queue.Enqueue(aParent);
        while (queue.Count > 0)
        {
            var c = queue.Dequeue();
            if (c.name == aName)
                return c;
            foreach (Transform t in c)
                queue.Enqueue(t);
        }

        return null;
    }
    */

    //Depth-first search
    public static Transform FindDeepChild(this Transform aParent, string aName)
    {
        foreach(Transform child in aParent)
        {
            if(child.name == aName )
                return child;
            var result = child.FindDeepChild(aName);
            if (result != null)
                return result;
        }
        return null;
    }

}