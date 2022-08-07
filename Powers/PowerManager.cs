using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using static MyExtensions.MyExtensions;

public class PowerManager : MonoBehaviour
{
    [SerializeField] List<Power> _powerList;
    //int _currentpowerSelected=0;
    static PowerCommand Spell2; //Spell1, 
    //int _spellIndexSpell1 = 0;
    //int _spellIndexSpell2 = 0;
    //int _spellPowerIndex = 0;
    //int _spellMax = 10;
    BoltLevel boltLevel1Ref;
    List<PowerCommand> _powerCommandsList = new List<PowerCommand>();
    SpriteRenderer _leftSpellSpriteRenderer;
    SpriteRenderer _rightSpellSpriteRenderer;
    TMP_Text _leftSpellQuantityText;
    TMP_Text _leftSpellQuantityText2;
    TMP_Text _rightSpellQuantityText;
    TMP_Text _rightSpellQuantityText2;
    public List<Sprite> _powerSpriteList;
    List<int> _powersUnlockedIndexs;
    List<int> _activePowersUnlocked;
    int _prevPowerIndex;
    int _activePowerIndex;
    int _nextPowerIndex;
    static int _powerCommandCountLeft;
    SpriteRenderer _prevSpellSpriteRenderer;
    SpriteRenderer _nextSpellSpriteRenderer;
    static string powerID = "PowerCount";

    static bool _isTextDisplayDirty;

    public static int PowerCountLeft()
    {
        return _powerCommandCountLeft;
    }
    
    public static void PowerCountDecrement()
    {
        if(_powerCommandCountLeft>0)
            _powerCommandCountLeft--;
        _isTextDisplayDirty = true ;
    }

    public static void PowerCountIncrement()
    {
        //MAX = 8
        if(_powerCommandCountLeft<8)
            _powerCommandCountLeft++;
        _isTextDisplayDirty = true;
    }

    public static void SetPowerCount(int value)
    {
        _powerCommandCountLeft = value;
    }

    public static void SetPowerCountInPrefs()
    {
        SetPrefPowerCount(_powerCommandCountLeft);
    }
    
    public static int GetPrefPowerCount()
    {
        int _defaultPowerCount = 0;    
        int powerCount = PlayerPrefs.GetInt(powerID, _defaultPowerCount);
        if (powerCount < 0) powerCount = 0;
        return powerCount;
    }

    public static void SetPrefPowerCount(int newPowerCount)
    {
        PlayerPrefs.SetInt(powerID, newPowerCount);
    }

    public static void PowercountRestartLevelCalled()
    {
        _powerCommandCountLeft = GetPrefPowerCount();
        _isTextDisplayDirty = true;
    }


    void Awake()
    {
        
        _powerCommandsList.Clear();
        _powerCommandsList.Add(new PerformNone());
        _powerCommandsList.Add(new StarRevealPassive());
        _powerCommandsList.Add(new CakeRevealPassive());
        _powerCommandsList.Add(new PerformNone());
        
        _powerCommandsList.Add(new PerformFireball());
        _powerCommandsList.Add(new PerformSuperJump());
        _powerCommandsList.Add(new PerformLightning());
        _powerCommandsList.Add(new PerformNone());
        
        _powerCommandsList.Add(new PerformShield());
        _powerCommandsList.Add(new PerformBomb());
        _powerCommandsList.Add(new PerformTeleport());
        _powerCommandsList.Add(new PerformNone());
        
        
        //get from prefs
        _powerCommandCountLeft = GetPrefPowerCount();
        
        
        _isTextDisplayDirty = false;
        //_powerCommandsList.Add(new PerformWaterFall());

//            _leftSpellSpriteRenderer = GameObject.Find("LeftSlotSprite").GetComponent<SpriteRenderer>();
        _rightSpellSpriteRenderer = GameObject.Find("RightSlotSprite").GetComponent<SpriteRenderer>();
//            _leftSpellQuantityText = GameObject.Find("LeftSpellQuantity").GetComponent<TMP_Text>();
//            _leftSpellQuantityText2 = GameObject.Find("LeftSpellQuantity (1)").GetComponent<TMP_Text>();
        _rightSpellQuantityText = GameObject.Find("RightSpellQuantity").GetComponent<TMP_Text>();
        _rightSpellQuantityText2 = GameObject.Find("RightSpellQuantity (1)").GetComponent<TMP_Text>();

        _prevSpellSpriteRenderer = GameObject.Find("PrevSlotSprite").GetComponent<SpriteRenderer>();
        _nextSpellSpriteRenderer = GameObject.Find("NextSlotSprite").GetComponent<SpriteRenderer>();

        //_spellMax = _powerCommandsList.Count-1;
        
        //_spellIndexSpell1 = 0;
        int _spellIndexSpell2 = 0;
        //Spell1 = _powerCommandsList[_spellIndexSpell1];
        Spell2 = _powerCommandsList[_spellIndexSpell2];
        //_leftSpellSpriteRenderer.sprite = _powerSpriteList[_spellIndexSpell1];
        _rightSpellSpriteRenderer.sprite = _powerSpriteList[_spellIndexSpell2];
        
        //Spell1.Initialise();
        Spell2.Initialise();

        //UpdateQuantityText(Spell1, _leftSpellQuantityText);
        UpdateQuantityText(Spell2, _rightSpellQuantityText);

        //_leftSpellQuantityText2.SetText(_leftSpellQuantityText.text);
        _rightSpellQuantityText2.text = _rightSpellQuantityText.text;

        boltLevel1Ref = GetComponentInChildren<BoltLevel>();

        _activePowerIndex = 0;
        _powersUnlockedIndexs = new List<int>();
        _activePowersUnlocked = new List<int>();
        UpdatePowersUnlockedList();
        UpdateActivePowersUnlockedList();
        
        //When starting?
        Spell2ChangeUp();

        Log($" UPDATING POWER ","red");
        foreach (var things in _powersUnlockedIndexs)
        {
            Log($" list of indexs {things} {_powerCommandsList[things].GetPowerName()} ","red");    
            
        }
        
        //_spellPowerIndex = 0;
        //RightSlotSprite
    }

    void UpdateActivePowersUnlockedList()
    {
        _activePowersUnlocked.Clear();
        _activePowersUnlocked.Add(0);
        foreach (var power in _powersUnlockedIndexs.Where(power => _powerCommandsList[power].isQuantity()))
        {
            _activePowersUnlocked.Add(power);
        }
    }
    
    void GetPreviousActivePower(int currentIndex,out int previousIndex, out int newCurrentIndex,out int nextIndex)
    {
        int rv = 0;
        previousIndex = 0;
        newCurrentIndex = 0;
        nextIndex = 0;
        
        if (_activePowersUnlocked.Count < 1) return;
        
        previousIndex = currentIndex;
        newCurrentIndex = currentIndex;
        nextIndex = currentIndex;
        
        //Skip zero on next.
        newCurrentIndex--;
        if (newCurrentIndex < 1 )
        {
            newCurrentIndex = _activePowersUnlocked.Count-1;
        }

        previousIndex = newCurrentIndex - 1;
        if (previousIndex < 1)
            previousIndex = _activePowersUnlocked.Count-1;
    }


    void GetNextActivePower(int currentIndex,out int previousIndex, out int newCurrentIndex,out int nextIndex)
    {
        int rv = 0;
        previousIndex = 0;
        newCurrentIndex = 0;
        nextIndex = 0;
        
        if (_activePowersUnlocked.Count < 1) return;
        
        previousIndex = currentIndex;
        newCurrentIndex = currentIndex;
        nextIndex = currentIndex;
        
        //Skip zero on next.
        newCurrentIndex++;
        if (newCurrentIndex > (_activePowersUnlocked.Count-1))
        {
            newCurrentIndex = 1;
        }

        nextIndex = newCurrentIndex + 1;
        if (nextIndex > (_activePowersUnlocked.Count-1))
            nextIndex = 1;
    }

    bool IsKittenUnlocked(int catIdentity)
    {
        string key = catIdentity + "Unlocked";
        int isUnlocked = PlayerPrefs.GetInt(key);
        //Debug.Log($"{catIdentity} {isUnlocked}");
        if (isUnlocked == 1) return true;
        return false;
    }

    public void UpdatePowersUnlockedList()
    {
        _powersUnlockedIndexs.Clear();
        
        for (int catid = 0; catid < 12; catid++)
        {
            if (IsKittenUnlocked(catid))
            {
                _powersUnlockedIndexs.Add(catid);
            }
        }

        foreach (var powerindx in _powersUnlockedIndexs)
        {
            Log($" cat at index {powerindx} is unlocked, init all the spells?");
            _powerCommandsList[powerindx].Initialise();
        }
        
    }
    
    
    //Index in = power list index.
    public int GetNextFreeSpellIndex(int indexPassedIn)
    {
        if (_powersUnlockedIndexs.Count < 1) return 0;
        indexPassedIn++;
        if (indexPassedIn > _powersUnlockedIndexs.Count - 1) return 0;
        return _powersUnlockedIndexs[indexPassedIn];
/*
        int testIndex = indexPassedIn+1;
        if (testIndex > _spellMax) testIndex = 0;
        if ((testIndex == _spellIndexSpell1) || (testIndex == _spellIndexSpell2))
            testIndex++;
        if (testIndex > _spellMax) testIndex = 0;
        return testIndex;*/
    }
/*
    public int GetNextFreeSpellIndexAllAvailable(int indexPassedIn)
    {
        int testIndex = indexPassedIn+1;
        if (testIndex > _spellMax) testIndex = 0;
        //if ((testIndex == _spellIndexSpell1) || (testIndex == _spellIndexSpell2))
        if (testIndex == _spellIndexSpell2)
            testIndex++;
        if (testIndex > _spellMax) testIndex = 0;
        return testIndex;
    }
*/
    void UpdateQuantityText(PowerCommand spell, TMP_Text quantityText)
    {
        //Log($"<color=red> Spell quant={spell.GetQuantity()} pass?{spell.isPassive()} inf=?{spell.isInfinite()} </color>");

        if ((spell.isInfinite()) || (spell.isPassive()))
            quantityText.text = "Inf";
        else
        {
            Log($"<color=red> Spell quant={spell.GetQuantity()} </color>");
            quantityText.text = spell.GetQuantity().ToString();
        }
        
    }
    
    /*public void Spell1ChangeUp()
    {
        Spell1.Reset();
        _spellIndexSpell1 = GetNextFreeSpellIndex(_spellIndexSpell1);
        Spell1 = _powerCommandsList[_spellIndexSpell1];
        _leftSpellSpriteRenderer.sprite = _powerSpriteList[_spellIndexSpell1];
        Spell1.Initialise();
        UpdateQuantityText(Spell1, _leftSpellQuantityText);
    }*/
    
    public void Spell2ChangeDown()
    {
        
        
        Spell2.Reset();
        Log($" Spell2ChangeDown Called {Time.time}");
        
        GetPreviousActivePower(_activePowerIndex,out int previousIndex,out int newCurrentIndex,out int nextIndex);
        Log($" APIin = {_activePowerIndex} prev={previousIndex} new={newCurrentIndex} next{nextIndex}");
        _activePowerIndex = newCurrentIndex;
        _prevPowerIndex = previousIndex;
        _nextPowerIndex = nextIndex;
        
        
        
        //_activePowerIndex = GetNextSpellUnlocked();
        //_spellPowerIndex = GetNextSpellUnlocked();
        
        //Update the UI
        int activePowerIdxToCommandIdx = _activePowersUnlocked[_activePowerIndex]; 
        Spell2 = _powerCommandsList[activePowerIdxToCommandIdx];
        Spell2.Initialise();
        UpdateQuantityText(Spell2, _rightSpellQuantityText);
        _rightSpellQuantityText2.text = _rightSpellQuantityText.text;

        if (_rightSpellSpriteRenderer == null) return;
        _rightSpellSpriteRenderer.sprite =_powerSpriteList[_activePowersUnlocked[_activePowerIndex]]; 
        _prevSpellSpriteRenderer.sprite = _powerSpriteList[_activePowersUnlocked[_prevPowerIndex]];
        _nextSpellSpriteRenderer.sprite = _powerSpriteList[_activePowersUnlocked[_nextPowerIndex]];
    }

    
    public void Spell2ChangeUp()
    {
        Spell2.Reset();
        Log($" Spell2ChangeUp Called {Time.time}");
        
        GetNextActivePower(_activePowerIndex,out int previousIndex,out int newCurrentIndex,out int nextIndex);
        Log($" APIin = {_activePowerIndex} prev={previousIndex} new={newCurrentIndex} next{nextIndex}");
        Log($" the length = {_activePowersUnlocked.Count}");
        _activePowerIndex = newCurrentIndex;
        _prevPowerIndex = previousIndex;
        _nextPowerIndex = nextIndex;
        
        //_activePowerIndex = GetNextSpellUnlocked();
        //_spellPowerIndex = GetNextSpellUnlocked();
        
        //Update the UI
        int activePowerIdxToCommandIdx;
        if (_activePowerIndex < _activePowersUnlocked.Count)
        {
            activePowerIdxToCommandIdx = _activePowersUnlocked[_activePowerIndex];
        }
        else
        {
            activePowerIdxToCommandIdx = 0;
            _activePowerIndex = 0;
            _nextPowerIndex = 0;
        }

        Spell2 = _powerCommandsList[activePowerIdxToCommandIdx];
        Spell2.Initialise();
        UpdateQuantityText(Spell2, _rightSpellQuantityText);
        _rightSpellQuantityText2.text = _rightSpellQuantityText.text;
        
        if (_rightSpellSpriteRenderer == null) return;
        
        _rightSpellSpriteRenderer.sprite =_powerSpriteList[_activePowersUnlocked[_activePowerIndex]]; 
        _prevSpellSpriteRenderer.sprite = _powerSpriteList[_activePowersUnlocked[_prevPowerIndex]];
        _nextSpellSpriteRenderer.sprite = _powerSpriteList[_activePowersUnlocked[_nextPowerIndex]];

    }

    public static void IncrementSpellQuantity()
    {
        //Spell1.IncrementQuantity();
        
        //Spell2.IncrementQuantity();

        _powerCommandCountLeft++;
        _isTextDisplayDirty = true;
    }


    public static void Spell1Performed()
    {
        //Spell1.Execute();
        
    }
    public static void Spell2Performed()
    {
        Spell2.Execute();
    }

    void HandleTimedModeUpdates(PowerCommand command)
    {
        if (command.isTimedMode())
        {
            if(command.isActive())
            {
                command.EnergyTimerDecrease(Time.deltaTime);
                boltLevel1Ref.SetGreyPercentLevel(100.0f-command.GetEnergyAsPercentage());
            }
            else
            {
                command.EnergyTimerIncrease(Time.deltaTime/3.0f);
                boltLevel1Ref.SetGreyPercentLevel(100.0f-command.GetEnergyAsPercentage());
            }
        }
    }

    void Update()
    {
        //HandleTimedModeUpdates(Spell1);
        HandleTimedModeUpdates(Spell2);
        if (_isTextDisplayDirty)
        {
            _isTextDisplayDirty = false;
            Spell2ChangeUp();
            Spell2ChangeDown();
        }
    }
}
