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
    [Tooltip("초당 공격 횟수")]
    public float AttackSpeed = 1.2f;
}
