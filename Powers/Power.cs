using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Power",menuName = "Powers",order = 51 )]
public class Power : ScriptableObject
{
    public enum PowerActivationType
    {
        Infinite,
        TimedOnly,
        Quantity,
        Passive
    };

    public enum PowerName
    {
        None,
        CakeReveal,
        SeeingStars,
        Fireball,
        JumpHeight,
        Lightning,
        Bomb,
        Shield,
        Teleport,
    };

    [SerializeField] PowerName powerName;
    [SerializeField] PowerActivationType powerActivationType;
    [SerializeField] float cooldownTimer;
    [SerializeField] int quantity;

    public string Powername => powerName.ToString();
    public string PowerActivation => powerActivationType.ToString();
    public float CooldownTimer => cooldownTimer;
    public int Quantity => quantity;

}
