using System;
using UnityEngine;

[Serializable]
public class PlayerStat
{
    [Header("Event")]
    public Action<float> PlayerStaminaChanged;
    public Action<float> PlayerHealthPointChanged;

    [Header("Basic Stat")]
    public float MaxHealthPoint;
    private float _currentHealthPoint;
    public float CurrentHealthPoint
    {
        get => _currentHealthPoint;
        set 
        { 
            _currentHealthPoint = Mathf.Clamp(value, 0f, MaxHealthPoint);
            PlayerHealthPointChanged?.Invoke(_currentHealthPoint);
        }
    }

    [Header("Movement & Jump")]
    public float CurrentMoveSpeed;
    public float MoveSpeed;
    public float RunSpeed;
    public float RunStaminaCostPerSecond;
    public float JumpPower;
    public float JumpStaminaCost;
    public float Gravity;

    [Header("Rotation")]
    public float RotateSpeed;
    public float YRotationMax;
    public float YRotationMin;

    [Header("Attack")]
    [Tooltip("초당 공격 횟수")]
    public float AttackSpeed = 1.2f;
    public float AttackStaminaCost;

    [Header("Stamina")]
    public float MaxStamina;

    private float _currentStamina;
    public float CurrentStamina
    {
        get => _currentStamina;
        set 
        { 
            _currentStamina = Mathf.Clamp(value, 0f, MaxStamina);
            PlayerStaminaChanged?.Invoke(_currentStamina);
        }
    }
    public float StaminaRecoveryForSecond;
}
