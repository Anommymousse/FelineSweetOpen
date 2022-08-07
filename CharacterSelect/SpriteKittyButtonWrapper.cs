using System.Collections;
using System.Collections.Generic;
using DarkTonic.MasterAudio;
using UnityEngine;

using static MyExtensions.MyExtensions;

public class SpriteKittyButtonWrapper : MonoBehaviour
{
    CreateCatIdForMainGame _createCatIdForMainGameRef;
    UIStartLevelButton _uiStartLevelButtonRef;
    BubbleUnlock _BubbleUnlockRef;
    
    // Start is called before the first frame update
    void Start()
    {
        _uiStartLevelButtonRef = GetComponent<UIStartLevelButton>();
        _createCatIdForMainGameRef = GetComponent<CreateCatIdForMainGame>();
        _BubbleUnlockRef = GetComponentInParent<BubbleUnlock>();
    }

    void OnBubbleButtonPress()
    {
        
    }

    void OnKittyWithNoBubble()
    {
        MasterAudio.PlaySound("Button");
        _createCatIdForMainGameRef.SetCatIDObject();
        StartCoroutine(LoadAfterALittleBit());
        
    }

    IEnumerator LoadAfterALittleBit()
    {
        yield return new WaitForSeconds(1.5f);
        _uiStartLevelButtonRef.LoadLevel();
    }

    public void TryUnlockOrSelectCat()
    {
        Log($"<color=red>TryToUnlockOrSelectCat</color>");
        if (_BubbleUnlockRef.IsKittenUnlocked(_BubbleUnlockRef.GetCatID()))
        {
            Log($"<color=red>No bubble kittey</color>");
            OnKittyWithNoBubble();
        }
        else
        {
            Log($"<color=red>Try To unlock kittey</color>");
           _BubbleUnlockRef.TryToUnlockTheKitten();
        }
    }
}
