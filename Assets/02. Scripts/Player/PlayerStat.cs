using System;
using UnityEngine;

[Serializable]
public class PlayerStat
{
    [Header("Basic Stat")]
    public int Health;


    [Header("Movement")]
    public float MoveSpeed;
    public float JumpPower;
    public float Gravity;

    [Header("Rotation")]
    public float RotateSpeed;
    public float YRotationMax;
    public float YRotationMin;


    [Header("Attack")]
    public readonly float ATTACK_COOLTIME = 0.6f;
}
