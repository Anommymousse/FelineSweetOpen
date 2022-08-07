using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    
    public abstract class PowerCommand
    {
        Power.PowerName _name;
        //int _quantity = 0; //No longer used
        //int _quantityMax = 8;
        float _energy = 0.0f;
        float _energyMax = 5.0f;
        
        //main functions.
        public abstract void Execute();
        public abstract void Initialise();
        public abstract void Reset();
        
        public void SetPowerName(Power.PowerName newName) => _name = newName;
        public Power.PowerName GetPowerName() => _name;

        //Timed energy things.
        bool _isTimed;
        bool _isActive;
        
        //Is infinite Type
        bool _isInfiniteType;
        bool _isPassivePowerType;
        bool _isQuanityType;

        public bool isPassive() => _isPassivePowerType;
        public void SetPassive(bool newState) => _isPassivePowerType = newState;
        
        public bool isInfinite() => _isInfiniteType;
        public void SetInifinty(bool newState) => _isInfiniteType = newState;
        
        public bool isActive() => _isActive;
        public void SetActive(bool newActive) => _isActive = newActive;

        public bool isQuantity() => _isQuanityType;
        public void SetQuanityType(bool newActive) => _isQuanityType = newActive;

        
        public bool isTimedMode() => _isTimed;
        public void SetTimedMode(bool timed) => _isTimed = timed;
        public void SetEnergy(float newEnergy) => _energy = newEnergy;
        public void SetMaxEnergy(float newEnergy) => _energyMax = newEnergy;
        public float GetEnergy() => _energy;
        public void EnergyReset() => _energy = _energyMax;
        public float GetEnergyAsPercentage() => (_energy / _energyMax) * 100.0f;
        
        //Quantity based powers
        //public void SetQuanity(int newQuantity) => _quantity = newQuantity;
        //public void SetMaxQuanity(int newQuantityMax) => _quantityMax = newQuantityMax;
        public int GetQuantity() => PowerManager.PowerCountLeft();
        //public int GetMaxQuantity() => 8;
        public void DecrementQuantity() => PowerManager.PowerCountDecrement();
        public void IncrementQuantity() => PowerManager.PowerCountIncrement();

        public void EnergyTimerIncrease(float energyIncrease)
        {
            _energy += energyIncrease;
            if (_energy > _energyMax)
                _energy = _energyMax;
        }
        public void EnergyTimerDecrease(float energyReduction)
        {
            _energy -= energyReduction;
            if (_energy < 0.01f)
            {
                _energy = 0.0f;
                SetActive(false);
                //CharacterController2D.SetGravityReverse(false);
            }
        }
        
    }

    public class CakeRevealPassive : PowerCommand
    {
        bool _alreadyInitialised = false;
        
        public override void Execute()
        {
        }

        public override void Initialise()
        {
            if (_alreadyInitialised == false)
            {
                SetPowerName(Power.PowerName.CakeReveal);
                SetPassive(true);
                Player.SetCakeRevealPassiveOn();
                _alreadyInitialised = true;
            }
        }
        public override void Reset()
        {
            Player.SetCakeRevealPassiveOff();
        }
    }

    public class StarRevealPassive : PowerCommand
    {
        bool _alreadyInitialised = false;
            
        public override void Execute()
        {
        }

        public override void Initialise()
        {
            if (_alreadyInitialised == false)
            {
                SetPowerName(Power.PowerName.SeeingStars);
                SetPassive(true);
                Player.SetStarRevealPassiveOn();
                _alreadyInitialised = true;
            }
        }

        public override void Reset()
        {
            Player.SetStarRevealPassiveOff();
        }
    }


    public class ScrollRevealPassive : PowerCommand
    {
        bool _alreadyInitialised = false;
        
        public override void Execute()
        {
        }

        public override void Initialise()
        {
            if (_alreadyInitialised == false)
            {
                Player.SetScrollRevealPassiveOn();
                _alreadyInitialised = true;
            }
        }

        public override void Reset()
        {
            Player.SetScrollRevealPassiveOff();
        }
    }

/*
    public class PerformGravityReverse : PowerCommand
    {
        bool _alreadyInitialised = false;
        Player _player;

        public override void Execute()
        {
            SetTimedMode(true);
            Debug.Log("<color=red> Reverse grav </color>");
            if (_alreadyInitialised == false)
                Initialise();
            if (CharacterController2D.GetGravityReverse() == false)
            {
                SetActive(true);
                CharacterController2D.SetGravityReverse(true);
            }
            else
            {
                SetActive(false);
                CharacterController2D.SetGravityReverse(false);
            }
            
        }
        
        public override void Initialise()
        {
            SetPowerName(Power.PowerName.Gravity);
            _player = GameObject.Find("Player").GetComponent<Player>();
            SetTimedMode(true);
            _alreadyInitialised = true;
            SetMaxEnergy(0.75f);
            EnergyReset();
            SetInifinty(true);
        }

        public override void Reset()
        {
            SetPowerName(Power.PowerName.None);
            CharacterController2D.SetGravityReverse(false);
            SetTimedMode(false);
            EnergyReset();
        }
        
    }
*/

    public class PerformWaterFall : PowerCommand
    {
        public override void Execute()
        {
            Debug.Log("<color=red> Water fall </color>");
        }

        public override void Initialise()
        {
            SetInifinty(true);
        }

        public override void Reset()
        {
        }
    }

    public class PerformNone  : PowerCommand
    {
        public override void Execute()
        {
            
        }

        public override void Initialise()
        {
            SetPowerName(Power.PowerName.None);
            SetInifinty(true);
        }

        public override void Reset()
        {
            SetPowerName(Power.PowerName.None);
        }
    }

    public class PerformShield : PowerCommand
    {
        public override void Execute()
        {
            if (PowerManager.PowerCountLeft() > 0)
            {
                GenericUnlockAchievement.UnlockAchievement("PowerShield");
                if (ShieldControl.GetShieldState()) return;
                
                Debug.Log("<color=red> Shield </color>");
                var playerMovement = GameObject.Find("Player").GetComponent<Player>();
                playerMovement.PlayerCastsShield();
                DecrementQuantity();
            }
            else
            {
                var playerMovement = GameObject.Find("Player").GetComponent<Player>();
                playerMovement.PlayersCastsDud();
            }
        }

        public override void Initialise()
        {
            SetPowerName(Power.PowerName.Shield);
            SetQuanityType(true);
            //SetQuanity(8);
            SetInifinty(false);
        }

        public override void Reset()
        {
            SetPowerName(Power.PowerName.None);
        }
    }

    public class PerformFireball : PowerCommand
    {
        public override void Execute()
        {
            if (GetQuantity() > 0)
            {
                Debug.Log("<color=red> Fireball </color>");
                var player = GameObject.Find("Player").GetComponent<Player>();
                player.PlayerCastsFireball();
                DecrementQuantity();
                if(!DifficultyLevel.GetDifficulty().Contains("Custom"))
                GenericUnlockAchievement.UnlockAchievement("PowerFireball");
            }
            else
            {
                var playerMovement = GameObject.Find("Player").GetComponent<Player>();
                playerMovement.PlayersCastsDud();
            }
        }

        public override void Initialise()
        {
            SetPowerName(Power.PowerName.Fireball);
            SetQuanityType(true);
            //SetQuanity(3);
            SetInifinty(false);
            SetPassive(false);
        }

        public override void Reset()
        {
            SetPowerName(Power.PowerName.None);
        }
    }

    public class PerformTeleport : PowerCommand
    {
        public override void Execute()
        {
            if (GetQuantity() > 0)
            {
                if(!DifficultyLevel.GetDifficulty().Contains("Custom"))
                GenericUnlockAchievement.UnlockAchievement("PowerTeleport");
                Debug.Log("<color=red> Teleport </color>");
                var playerMovement = GameObject.Find("Player").GetComponent<Player>();
                playerMovement.PlayerCastsTeleport();
                DecrementQuantity();
            }
            else
            {
                var playerMovement = GameObject.Find("Player").GetComponent<Player>();
                playerMovement.PlayersCastsDud();
            }            
        }

        public override void Initialise()
        {
            SetPowerName(Power.PowerName.Teleport);
            SetQuanityType(true);
            //SetQuanity(8);
            SetInifinty(false);
            SetPassive(false);

        }

        public override void Reset()
        {
            SetPowerName(Power.PowerName.None);
        }
        
    }

public class PerformSuperJump : PowerCommand
    {
        public override void Execute()
        {
            Debug.Log("<color=red> Super Jump </color>");
        }

        public override void Initialise()
        {
            if(!DifficultyLevel.GetDifficulty().Contains("Custom"))
            GenericUnlockAchievement.UnlockAchievement("PowerSpeed");
            SetPowerName(Power.PowerName.JumpHeight);
            CharacterController2D.SetSuperJump(true);
            SetInifinty(true);
        }

        public override void Reset()
        {
            SetPowerName(Power.PowerName.None);
            CharacterController2D.SetSuperJump(false);
        }
    }


    public class PerformBomb : PowerCommand
    {
        public override void Execute()
        {
            var bombPresent = GameObject.FindGameObjectWithTag("Bomb");
            if(bombPresent==null)
                if (GetQuantity() > 0)
                {
                    if(!DifficultyLevel.GetDifficulty().Contains("Custom"))
                    GenericUnlockAchievement.UnlockAchievement("PowerBomb");
                    DecrementQuantity();
                    Debug.Log("<color=red> BooooomB cast </color>");
                    var playerMovement = GameObject.Find("Player").GetComponent<Player>();
                    playerMovement.PlayerCastsBomb();
                }
                else
                {
                    var playerMovement = GameObject.Find("Player").GetComponent<Player>();
                    playerMovement.PlayersCastsDud();
                }
        }

        public override void Initialise()
        {
            SetPowerName(Power.PowerName.Bomb);
            SetQuanityType(true);
            //SetQuanity(8);
            SetInifinty(false);
            SetPassive(false);
        }

        public override void Reset()
        {
            SetPowerName(Power.PowerName.None);
        }
    }

    public class PerformLightning : PowerCommand
    {
        public override void Execute()
        {
            if (GetQuantity() > 0)
            {
                if(!DifficultyLevel.GetDifficulty().Contains("Custom"))
                GenericUnlockAchievement.UnlockAchievement("PowerLightning");
                Debug.Log("<color=red> Lightning </color>");
                var playerMovement = GameObject.Find("Player").GetComponent<Player>();
                playerMovement.PlayerCastsLightning();
                DecrementQuantity();
            }
            else
            {
                var playerMovement = GameObject.Find("Player").GetComponent<Player>();
                playerMovement.PlayersCastsDud();
            }
        }

        public override void Initialise()
        {
            SetPowerName(Power.PowerName.Lightning);
            SetQuanityType(true);
            //SetQuanity(8);
            SetInifinty(false);
            SetPassive(false);
        }

        public override void Reset()
        {
            SetPowerName(Power.PowerName.None);
        }
    }






    





