using Photon.Pun;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamaged
{
    public PlayerStat Stat;
    private Dictionary<Type, PlayerAbility> _abilitiesCache = new();

    private void Awake()
    {
        if (Stat == null)
        {
            Stat = new PlayerStat();
        }
        // PlayerStat 초기화
        Stat.CurrentHealthPoint = Stat.MaxHealthPoint;
        Stat.CurrentStamina = Stat.MaxStamina;
        Stat.CurrentMoveSpeed = Stat.MoveSpeed;
    }

    public T GetAbility<T>() where T : PlayerAbility
    {
        var type = typeof(T);

        if (_abilitiesCache.TryGetValue(type, out PlayerAbility ability))
        {
            return ability as T;
        }

        // 게으른 초기화/로딩 -> 처음에 곧바로 초기화/로딩을 하는게 아니라
        //                    필요할때만 하는.. 뒤로 미루는 기법
        ability = GetComponent<T>();

        if (ability != null)
        {
            _abilitiesCache[ability.GetType()] = ability;

            return ability as T;
        }

        throw new Exception($"어빌리티 {type.Name}을 {gameObject.name}에서 찾을 수 없습니다.");
    }

    [PunRPC]
    public void Damaged(float damage)
    {
        Stat.CurrentHealthPoint -= damage;
        Debug.Log($"남은 체력 : {Stat.CurrentHealthPoint}");
    }
}