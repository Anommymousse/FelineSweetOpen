using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using DarkTonic.MasterAudio;
using DarkTonic.PoolBoss;
using DigitalRuby.ThunderAndLightning;
using MoreMountains.Feedbacks;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Networking.Types;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using static MyExtensions.MyExtensions;

[System.Serializable]
public class PlayerCastBuildEvent : UnityEvent<Vector3,float>
{
}

[System.Serializable]
public class PlayerDashEvent : UnityEvent<float>
{
}

public class Player : MonoBehaviour
{
    static Player _instance;
    static PlayerCastBuildEvent _playerCastBuildEvent = new PlayerCastBuildEvent();
    static PlayerDashEvent _playerCastsDashEvent = new PlayerDashEvent();
    public static PlayerCastBuildEvent GetPlayerBuildEvent => _playerCastBuildEvent;
    public static PlayerDashEvent GetPlayerDashEvent => _playerCastsDashEvent;
    public static Vector2 GetDashDirection => _dashDirection; 

    //Default power
    //DASH
    static bool isDashTriggered;
    float dashTriggeredAtTime;
    static Vector2 _dashDirection;

    public static Vector3 GetWorldLocation() => _instance.transform.position; 
    
    
    //Unlocked Powers section
    PowerManager _powerManagerRef;
    bool _gravityReverseActive;
    //bool _superJumpActive = false;
    //bool _superSpeedActive = false;
    //static float _superSpeedMult = 1.0f;
    PlayerInput _playerInput;
    //public void SetSuperJump() => _superJumpActive = true; 
    //public void SetSuperSpeed() => _superSpeedActive = true;
    public void SetGravityReverse() => _gravityReverseActive = true;
    
    static bool _canSeeHiddenStars;
    public static bool CanPlayerSeeStars() => _canSeeHiddenStars;
    static bool _canSeeHiddenCakes;    
    public static bool CanPlayerSeeCakes() => _canSeeHiddenCakes;


    //Feedbacks
    public static MMFeedbacks _jumpFeedbacks;
    public static MMFeedbacks _landFeedbacks;
    public static MMFeedbacks _gravityFeedback;
    public static MMFeedbacks _fireballFeedback;

    //Speed settings
    //static public void SetSuperSpeedMultiplier(float superSpeedMult) => _superSpeedMult = superSpeedMult;
    static public void SetSuperDashMultiplierOn()
    {
    }
    static public void SetSuperSpeedMultiplierOff()
    {
    }

    public static void PlayerGetsKey()
    {
        OnPlayerGetsKey.Invoke();
    }

    
    //Passive Settings
    public static  void SetCakeRevealPassiveOn()
    {
        _canSeeHiddenCakes = true;
        if(!DifficultyLevel.GetDifficulty().Contains("Custom"))
        GenericUnlockAchievement.UnlockAchievement("SeeingCakes");
    }
    public static  void SetCakeRevealPassiveOff()
    {
        _canSeeHiddenCakes = false;
    }
    public static void SetScrollRevealPassiveOn()
    {
    }
    public static void SetScrollRevealPassiveOff()
    {
    }
    public static void SetStarRevealPassiveOn()
    {
        _canSeeHiddenStars = true;
        if(!DifficultyLevel.GetDifficulty().Contains("Custom"))
            GenericUnlockAchievement.UnlockAchievement("SeeingStars");
    }
    public static void SetStarRevealPassiveOff()
    {
        _canSeeHiddenStars = false;
    }

    //Level and resets.
    public Action OnPlayerLevelChange;
    public Action OnPlayerReset;
    
    //////////////////////////////////////////////////////////
    //Outside events.
    /// These are the events that interfaces with other stuff
    /// 
    /// Brick interactions, powers
    //////////////////////////////////////////////////////////

    //Powers Outside events
    public Action OnPlayerCastFireball;
    public Action OnPlayerCastLightning;
    public Action OnPlayerCastTeleport;
    public Action OnPlayerCastShield;
    public Action OnPlayerCastBomb;
    public Action OnPlayerCastWaterfall;
    public Action OnPlayerCastSuperDash;
    public static Action OnPlayerGetsKey;
    public Action OnPlayerJump;

    

    /// <Referencee>
    /// These are the references grabbed or set at the start
    /// </summary>
    public CharacterController2D controller;
    SpriteRenderer _spriteRenderer;
    Vector3 _startingPosition;

    //Toggles and settings    
    public float runSpeed = 40f;
    [SerializeField] float horizontalMove = 0f;
    bool _jump = false;
    bool _crouch = false;
    bool _jumpPerformedThisFrame;
    float _left;
    float _right;
    float _down;
    float _escape;
    bool _playerDying = false;
    static string _playerDeathParticles = "PlayerDeath";
    bool _playerHasKey;
    
    //Control of horizontal movement acceleration and deceleration.
    static float _movementPreviousFrame;
    //int _framesToMax = 6;
    //int frameToStop = 3;
    //int currentFrameCount = 0;

    //Speeds
    float stage1run = 0.10f;
    float stage2run = 0.20f;
    float stage3run = 0.45f;
    float stage4run = 0.7f;
    float stage5run = 0.9f;
    float stage6run = 1.0f;
    //Timers
    float stage1runTimeThreshold = 0.10f;
    float stage2runTimeThreshold = 0.2f;
    float stage3runTimeThreshold = 0.3f;
    float stage4runTimeThreshold = 0.4f;
    float stage5runTimeThreshold = 0.45f;
    float stage6runTimeThreshold = 0.5f;
    
    //VFX Adjustments
    float y_offsetJumpVFX = 0.15f;
    
    //Mhhhmmmmm tasty spaget
    BrickMap _bricksReference;


    void Start()
    {
        StartupPlayer();
        _instance = this;
    }
    
    void OnEnable()
    {
        SetupPlayerControls();
        SetupPlayerEvents();
        SetupExternalEvents();
        SetupPlayerPowers();
        
        _powerManagerRef = GetComponentInChildren<PowerManager>();
    }

    void SetupExternalEvents()
    {
        controller.GetRoofHitEvent.AddListener(PlayerHitsRoof);
        controller.GetJumpStartedEvent.AddListener(JumpStarted);
    }
    
    void OnDisable()
    {
        DisablePlayerControlEvents();
        DisablePlayerEvents();
        DisableExternalEvents();
        DisablePlayerPowers();
    }

    void DisableExternalEvents()
    {
        controller.GetRoofHitEvent.RemoveListener(PlayerHitsRoof);
    }

    void SetupPlayerPowers()
    {
        InitialiseActivePowers();
        InitialisePassivePowers();
    }

    void InitialisePassivePowers()
    {
    }

    void InitialiseActivePowers()
    {
    }

    void DisablePlayerPowers()
    {
    }
    
    public void PlayerCastsFireball() => PowerCast.CastsFireball(transform.position,_spriteRenderer.flipX);
    public void PlayerCastsLightning() => PowerCast.CastsLightning(transform.position,_spriteRenderer.flipX);
    void CacheRestartPositions() => _startingPosition = gameObject.transform.position;

    public void Restart()
    {
        _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        gameObject.transform.position = _startingPosition;
        _jumpPerformedThisFrame = false;
        _jump = false;
        _left = 0.0f;
        _right = 0.0f;
        _playerDying = false;
        _spriteRenderer.enabled = true;
        
        var boxcol = GetComponent<CapsuleCollider2D>();
        boxcol.enabled = true;

        PowerManager.PowercountRestartLevelCalled();
    }

    public void NewLevel()
    {
        StartupPlayer();
        gameObject.transform.position = _startingPosition;
        _jumpPerformedThisFrame = false;
        _jump = false;
        _left = 0.0f;
        _right = 0.0f;
        _playerDying = false;
        _spriteRenderer.enabled = true;
        var boxcol = GetComponent<CapsuleCollider2D>();
        boxcol.enabled = true;
        
    }

    void StartupPlayer()
    {
        _playerHasKey = false;
        _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        CacheRestartPositions();
        var root = gameObject.transform.root;
        _bricksReference = root.GetComponentInChildren<BrickMap>();
        //_levelLoaderReference = GameObject.Find("LevelManager").GetComponent<LevelLoader>();
        
        //Feedbacks
        _jumpFeedbacks = GameObject.Find("Feedback_Jump").GetComponent<MMFeedbacks>();
        //_gravityFeedback
        _landFeedbacks = GameObject.Find("Feedback_Landing").GetComponent<MMFeedbacks>();

        PlayerDashReset();
    }
    
    void PlayerHitsRoof(Vector3 arg0)
    {
        Debug.Log($" Roof hit point {arg0}");
    }

    void JumpStarted(Vector3 arg0)
    {
        Debug.Log("VFXJump");
        //Adjust position of particle...
        arg0.y += y_offsetJumpVFX;
        if(PoolBoss.IsReady)
            PoolBoss.SpawnInPool("VfxJump", arg0, Quaternion.identity);
        _jumpFeedbacks.PlayFeedbacks();
    }

    void DisablePlayerControlEvents()
    {
        _playerInput.actions["Build"].performed -= OnPlayerCastBuild;
        _playerInput.actions["Jump"].performed -= PlayerJump;
        _playerInput.actions["Spell2"].performed -= PlayerCastSpellAlt;
        _playerInput.actions["Spell2Up"].performed -= PlayerSpell2ChangeUp;
        _playerInput.actions["Esc"].performed -= EscapePressed;
        _playerInput.actions["BuildDown"].performed -= PlayerBuildBlockDown;
        _playerInput.actions["Dash"].performed -= PlayerDash;
    }
    
    void SetupPlayerControls()
    {
        //Controls Setup.
        _playerInput = GetComponent<PlayerInput>();
        _playerInput.actions["Build"].performed += OnPlayerCastBuild;
        _playerInput.actions["Left"].performed += context => _left = context.ReadValue<float>();
        _playerInput.actions["Left"].canceled += context => _left = 0.0f;
        _playerInput.actions["Right"].performed += context => _right = context.ReadValue<float>();
        _playerInput.actions["Right"].canceled += context => _right = 0.0f;
        _playerInput.actions["DashDirection"].performed += context => _dashDirection = context.ReadValue<Vector2>();
        _playerInput.actions["DashDirection"].canceled += context => _dashDirection = Vector2.zero;
        _playerInput.actions["BuildDown"].performed += PlayerBuildBlockDown;
        _playerInput.actions["Jump"].performed += PlayerJump;
        _playerInput.actions["Dash"].performed += PlayerDash;
        _playerInput.actions["Spell2"].performed += PlayerCastSpellAlt;
        _playerInput.actions["Spell2Up"].performed += PlayerSpell2ChangeUp;
        _playerInput.actions["Spell2Down"].performed += PlayerSpell2ChangeDown;
        _playerInput.actions["Esc"].performed += EscapePressed;
    }

    void PlayerSpell2ChangeDown(InputAction.CallbackContext obj)
    {
        _powerManagerRef.Spell2ChangeDown();
    }

    void DisablePlayerEvents()
    {
        OnPlayerReset -= Restart;
        OnPlayerLevelChange -= NewLevel;
        OnPlayerGetsKey -= OnPlayerGetsKeyForLevel;
    }
    void SetupPlayerEvents()
    {
        OnPlayerReset += Restart;
        OnPlayerLevelChange += NewLevel;
        OnPlayerGetsKey += OnPlayerGetsKeyForLevel;
    }

    void OnPlayerGetsKeyForLevel()
    {
        Debug.Log($"PLAYHER GOT KEY");
        BrickMap.CollectKey();
        _playerHasKey = true;
    }

    void PlayerBuildBlockDown(InputAction.CallbackContext obj)
    {
        var position = gameObject.transform.position;
        position.y += 0.3f;//?
 
        //Get direction.
        Vector2 direction;// = new Vector2();
        
        direction.x = 1;
        if (_spriteRenderer.flipX)
        {
            direction.x = -1;
        }

        position.y -= 1f;
        
        _playerCastBuildEvent.Invoke(position,direction.x);
    }

    void PlayerDash(InputAction.CallbackContext obj)
    {
        Debug.Log("Player Press dash");
        if (isDashTriggered == false)
        {
            Debug.Log($"<color=red>Dash triggered!</color>");
            isDashTriggered = true;
            dashTriggeredAtTime = Time.time;
            _playerCastsDashEvent.Invoke(dashTriggeredAtTime);
        }
    }

    public static void PlayerDashReset()
    {
        isDashTriggered = false;
    }

    void OnPlayerBuild()
    {
    }

    void PlayerSpell2ChangeUp(InputAction.CallbackContext obj)
    {
        Log($"<color=red> Ply2 spel up </color>");
        var catSelection = GetComponent<CatSelection>();
        if (catSelection != null)
        {
            Debug.Log($" cat selection testinc");
            //catSelection.TestInc();
        }
        else
        {
            Debug.Log($"cat selection null");
        }
        _powerManagerRef.Spell2ChangeUp();
    }
    
    void EscapePressed(InputAction.CallbackContext context)
    {
        var gameobject = GameObject.Find("PauseMenuManager");
        if (gameobject != null)
        {
            var comp = gameobject.GetComponent<PausePanelManager>();
            if(comp!=null)
                comp.PauseMenuToggle();
            else
            {
                Debug.Log($"<color=red> comp = null </color>");
            }
        }
    }

    void PlayerCastSpellAlt(InputAction.CallbackContext obj)
    {
        PowerManager.Spell2Performed();
        Debug.Log($"spell 2");
    }
    
    void OnPlayerCastBuild(InputAction.CallbackContext callbackContext)
    {
        Debug.Log("Build block");
        
        //_crouch
        //var brickComponent = GetComponentInParent<Bricks>();
        var position = gameObject.transform.position;
        position.y += 0.3f;//?
 
        //Get direction.
        Vector2 direction;// = new Vector2();
        direction.y = 0;
        direction.x = 1;
        if (_spriteRenderer.flipX)
        {
            direction.x = -1;
        }
        _bricksReference.ReverseKey(position,direction);
        
        _playerCastBuildEvent.Invoke(position,direction.x);
        //_bricksReference.BuildOrDestroy(position,direction);
        
    }

    void PlayerJump(InputAction.CallbackContext callbackContext)
    {
        _jumpPerformedThisFrame = true;
    }


    bool IsEditorMode()
    {
        var gameobj = GameObject.Find("EDITORMODE");
        if (gameobj != null)
            return true;
        return false;
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if(_playerHasKey)
            if (other.name.Contains("door") || other.name.Contains("Door"))
            {
                
                Debug.Log($"Player go door 1");
                if (IsEditorMode())
                {
                    SceneManager.LoadScene("Scenes/LevelEditor");
                    return;
                }
                
                //_playerCastBuildEvent.RemoveAllListeners();
                BrickMap.GoThroughDoor();
                OnPlayerLevelChange?.Invoke();
            }
        
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if(_playerHasKey)
            if (other.collider.name.Contains("Door"))
            {
                Debug.Log($"Player go door 2");
                if(IsEditorMode())
                {
                    SceneManager.LoadScene("Scenes/LevelEditor");
                    return;
                }
                
                BrickMap.GoThroughDoor();
                OnPlayerLevelChange?.Invoke();
            }
    }


    float GetVelocityStageAdjustment(int stageAchieved)
    {
        switch (stageAchieved)
        {
            case 0 :
                return 0.1f;
            case 1 :
                return stage1run;
            case 2:
                return stage2run;
            case 3:
                return stage3run;
            case 4:
                return stage4run;
            case 5:
                return stage5run;
            default:
                return stage6run;
        }
    }


    static int previousDirection = 0;
    static float timerHoldDirection = 0.0f;
    float timerhold;

    float HeldTimer(int direction)
    {
        if (previousDirection == direction)
        {
            timerHoldDirection += Time.deltaTime;
            timerhold = timerHoldDirection;
          //  Log($"Time Increasing {direction}");
            return timerHoldDirection;
        }
       // Log($"Time static {direction} old direction {previousDirection}");
        timerHoldDirection = 0.0f;
        previousDirection = direction;
        timerhold = timerHoldDirection;
        return timerHoldDirection;
    }
    
    
    float AdjustHorizontalMovementForInertia(float things)
    {
        if (things > 0)
        {
            if (HeldTimer(1) > stage6runTimeThreshold)
                return GetVelocityStageAdjustment(6);
            if (HeldTimer(1) > stage5runTimeThreshold)
                return GetVelocityStageAdjustment(5);
            if (HeldTimer(1) > stage4runTimeThreshold)
                return GetVelocityStageAdjustment(4);
            if (HeldTimer(1) > stage3runTimeThreshold)
                return GetVelocityStageAdjustment(3);
            if (HeldTimer(1) > stage2runTimeThreshold)
                return GetVelocityStageAdjustment(2);
            if (HeldTimer(1) > stage1runTimeThreshold)
                return GetVelocityStageAdjustment(1);
            HeldTimer(1);
            return  GetVelocityStageAdjustment(0);
        } 
        
        if(things<0)
        {
            if(HeldTimer(-1)>stage6runTimeThreshold)
                return GetVelocityStageAdjustment(6);
            if(HeldTimer(-1)>stage5runTimeThreshold)
                return GetVelocityStageAdjustment(5);
            if(HeldTimer(-1)>stage4runTimeThreshold)
                return GetVelocityStageAdjustment(4);
            if(HeldTimer(-1)>stage3runTimeThreshold)
                return GetVelocityStageAdjustment(3);
            if(HeldTimer(-1)>stage2runTimeThreshold)
                return GetVelocityStageAdjustment(2);
            if (HeldTimer(-1)>stage1runTimeThreshold)
                return GetVelocityStageAdjustment(1);
            HeldTimer(-1);
            return  GetVelocityStageAdjustment(0);
        }

        HeldTimer(0);
        return GetVelocityStageAdjustment(0);
    }
    
    // Update is called once per frame
    void Update ()
    {
        horizontalMove = 0;
        if(_left>0.01f) horizontalMove = -1;
        if (_right>0.01f) horizontalMove = 1;
        if ((_left > 0.01f) && (_right > 0.01f)) horizontalMove = 0;

       // Log($" horz={horizontalMove} left={_left} right={_right} ");

        float adjRunSpeed = AdjustHorizontalMovementForInertia(horizontalMove);
        //Log($" adjrunspd = {adjRunSpeed} ");

        horizontalMove *= adjRunSpeed*runSpeed;//* _superSpeedMult;
        
        /*if (_down > 0.01f)
            _crouch = true;
        else
            _crouch = false;*/
     
        //Could on a slow machine jump be called multiple times?
        if(_jumpPerformedThisFrame)
            _jump = true;
        
        _jumpPerformedThisFrame = false;
        
    }

    void FixedUpdate ()
    {
        // Move our character
        controller.Move(horizontalMove * Time.fixedDeltaTime, _crouch, _jump);
        _jump = false;
    }

    void IncrementSavedPlayerRespawnCounter()
    {
        int currentRespawns = PlayerPrefs.GetInt("PlayerRespawnCount");
        currentRespawns++;
        PlayerPrefs.SetInt("PlayerRespawnCount",currentRespawns);
    }
    internal void TimeKillThePlayer()
    {
        GenericUnlockAchievement.UnlockAchievement("DeathByTime");
        IncrementSavedPlayerRespawnCounter();
        StartCoroutine(DisplayDeath());
        MasterAudio.StopBus("PlayerSounds");
    }
    
    internal void KillThePlayer(string gameObjectNameKillingPlayer)
    {
        if(IsEditorMode())        
        {
            SceneManager.LoadScene("Scenes/LevelEditor");
        }
        
        Log($"Call to boop the player at {Time.time}");
        
        Log($"Booped by {gameObjectNameKillingPlayer} at {Time.time}","red");
        
        var shieldControl = GetComponentInChildren<ShieldControl>();
        shieldControl.DeactivateShield();
        
        if (ShieldControl.GetShieldState()==false)
        {
            BoopedSteamAchievement(gameObjectNameKillingPlayer);
            
            MasterAudio.StopBus("PlayerSounds");
            IncrementSavedPlayerRespawnCounter();
            StartCoroutine(DisplayDeath());
        }
    }

    
    
    
    static void BoopedSteamAchievement(string gameObjectNameKillingPlayer)
    {
        Log($"Starting boop achieve check! at {Time.time}","white");
        if (gameObjectNameKillingPlayer.Contains("Electric")) gameObjectNameKillingPlayer = "Electric";
        gameObjectNameKillingPlayer = gameObjectNameKillingPlayer.Replace("Left", "");
        gameObjectNameKillingPlayer = gameObjectNameKillingPlayer.Replace("Right", "");
        gameObjectNameKillingPlayer = gameObjectNameKillingPlayer.Replace("_", " ");
        gameObjectNameKillingPlayer = gameObjectNameKillingPlayer.Replace("Vert", " ");
        gameObjectNameKillingPlayer = gameObjectNameKillingPlayer.Replace("Horz", " ");
        //gameObjectNameKillingPlayer = gameObjectNameKillingPlayer.Replace("(Clone)", "");
        string[] splitString = gameObjectNameKillingPlayer.Split(" ");
        gameObjectNameKillingPlayer = splitString[0];
        //gameObjectNameKillingPlayer = Regex.Replace(gameObjectNameKillingPlayer, "[^0-9]", "");
        
        Log($" GAMEOBJECT = {gameObjectNameKillingPlayer} at {Time.time}","cyan");
        
        if (GenericUnlockAchievement.SteamAchievementsDictionary.TryGetValue(gameObjectNameKillingPlayer, out string value))
        {
            Log($"Attempting to unlock boop = {gameObjectNameKillingPlayer} achID= {value} at {Time.time}","red");
            GenericUnlockAchievement.UnlockAchievement(value);
        }
        
        //if (gameObjectNameKillingPlayer.Contains("Pumpkin")) GenericUnlockAchievement.UnlockAchievement("DeathPumpkin");
        //if (gameObjectNameKillingPlayer.Contains("Penguin")) GenericUnlockAchievement.UnlockAchievement("DeathPenguin");
        //if (gameObjectNameKillingPlayer.Contains("Mummy")) GenericUnlockAchievement.UnlockAchievement("MummyMash");
        
    }

    IEnumerator DisplayDeath()
    {
        if (!_playerDying)
        {
            _playerDying = true;
            
            yield return null;
            MasterAudio.PlaySound("whistle_slide_down_02");
            PoolBoss.SpawnOutsidePool(_playerDeathParticles, gameObject.transform.position, Quaternion.identity);
            //PoolBoss.SpawnInPool(_playerDeathParticles, gameObject.transform.position, Quaternion.identity);
            Time.timeScale = 0.0f;
            yield return FadePlayer();
            if(OnPlayerReset!=null)
                OnPlayerReset();
            PoolBoss.DespawnAllPrefabs();

            //Level manager takes care of this LevelLoader
            //by monitoring player reset and levelchange
            //if (_levelLoaderReference != null)
            //{
            
            Log($"<color=red> Player deaths/restarts/levels = {LevelLoader._reloadCounter} </color>");
            
            LevelLoader.ReloadLevel();
            LevelLoader.LevelreStart();
            //}

            Time.timeScale = 1.0f;
            
        }
    }

    IEnumerator FadePlayer()
    {
        _spriteRenderer.enabled = false;
        var boxcol = GetComponent<CapsuleCollider2D>();
        boxcol.enabled = false;
        yield return null;
    }

    public void PlayerCastsBomb()
    {
        var transformpos = transform.position;
        transformpos.y += 0.5f;
        var bomb = PoolBoss.SpawnInPool("PlayerBomb", transformpos, Quaternion.identity);
    }

    //Disabled.
    public void PlayerCastsTeleport()
    {
        var worldpos = transform.position;
        worldpos.x -= 0.4f;
        worldpos.y += 0.5f;
        
        var testBrickPosition = _bricksReference.NonHiddenTilemap.WorldToCell(worldpos);
        Debug.Log($"thinks player is noew at {testBrickPosition} ");

        for(int x=-1;x<2;x++)
        for (int y = -1; y < 2; y++)
        {
            var adjpos = testBrickPosition;
            adjpos.x += x;
            adjpos.y += y;
            Debug.Log($" x={adjpos.x} y={adjpos.y} {_bricksReference.NonHiddenTilemap.HasTile(adjpos)}");
        }
        
        var playerRenderer = GetComponent<SpriteRenderer>();
        if (playerRenderer.flipX)
        {
            testBrickPosition.x -= 2;
        }
        else
        {
            testBrickPosition.x += 2;
        }
        
        if (_bricksReference.NonHiddenTilemap.HasTile(testBrickPosition)==false)
        {
            MasterAudio.PlaySound("TeleporterStart");
            if (playerRenderer.flipX) testBrickPosition.x += 1;
            var aworldpos = _bricksReference.NonHiddenTilemap.CellToWorld(testBrickPosition);
            controller.Teleport(aworldpos);
            //MasterAudio.PlaySound("TeleporterLand");
        }
        else
        {
            Debug.Log($"Has tile :(");
        }        
    }

    public void PlayerCastsShield()
    {
        Debug.Log("Player casts SHIELD.");
        var shieldControl = GetComponentInChildren<ShieldControl>();
        shieldControl.ActivateShield();
    }

    public void PlayersCastsDud()
    {
        Debug.Log($"Spell empty?");
        MasterAudio.PlaySound("SpellEmpty");

    }

}