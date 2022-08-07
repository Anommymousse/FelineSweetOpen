using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomFaceSelect : MonoBehaviour
{
    [SerializeField] List<Sprite> _imageAnimalFaces;
    [SerializeField] List<RuntimeAnimatorController> _animatorControllers;
    static int cycle = 0;
    // Start is called before the first frame update
    void Start()
    {
        if (cycle >= _imageAnimalFaces.Count) cycle = 0;
        int whichToUse = cycle;
        var image = GetComponent <Image>();
        image.sprite = _imageAnimalFaces[whichToUse];
        var anim = GetComponent<Animator>();
        anim.runtimeAnimatorController = _animatorControllers[whichToUse];
        cycle++;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
