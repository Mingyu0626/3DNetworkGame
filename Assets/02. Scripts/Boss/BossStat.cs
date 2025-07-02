using System;
using UnityEngine;

[System.Serializable]
public class BossStat
{
    [Header("Basic Stat")]
    public float MaxHealthPoint;
    private float _currentHealthPoint;
    public float CurrentHealthPoint
    {
        get => _currentHealthPoint;
        set
        {
            _currentHealthPoint = Mathf.Clamp(value, 0f, MaxHealthPoint);
        }
    }

    [Header("Movement")]
    public float MoveSpeed;
    public float Gravity;

    [Header("Trace")]
    public float ContextToPatrolTime;
    public float DistanceForTrace;

    [Header("Attack")]
    public float DistanceForAttack;
    [Tooltip("초당 공격 횟수")]
    public float AttackSpeed;
    public float AttackRange;
    public float Damage;

    [Header("Respawn")]
    public float RespawnTime;

}
