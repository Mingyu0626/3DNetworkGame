using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class Player : MonoBehaviour, IDamaged
{
    public PlayerStat Stat;
    private Dictionary<Type, PlayerAbility> _abilitiesCache = new();
    private Animator _animator;
    private PhotonView _photonView;

    private WaitForSeconds _deathTimer = new WaitForSeconds(5f);

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

        _animator = GetComponent<Animator>();
        _photonView = GetComponent<PhotonView>();
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
        // GetAbility<PlayerUIAbility>().Refresh();

        if (Stat.CurrentHealthPoint == 0)
        {
            _photonView.RPC(nameof(Dead), RpcTarget.All);
        }
    }

    [PunRPC]
    public void Dead()
    {
        if (_photonView.IsMine)
        {
            InputManager.Instance.IsInputBlocked = true;
        }
        _animator.SetTrigger("Die");
        StartCoroutine(DeathTimerCoroutine());
    }
    private IEnumerator DeathTimerCoroutine()
    {
        yield return _deathTimer;
        if (_photonView.IsMine)
        {
            Vector3 spawnPosition = PlayerSpawnManager.Instance.GetRandomSpawnPosition();
            _photonView.RPC(nameof(Respawn), RpcTarget.All, spawnPosition);
        }
    }

    [PunRPC]
    private void Respawn(Vector3 spawnPosition)
    {
        if (_photonView.IsMine)
        {
            InputManager.Instance.IsInputBlocked = false;
        }
        _animator.SetTrigger("Respawn");
        transform.position = spawnPosition;
        Stat.CurrentHealthPoint = Stat.MaxHealthPoint;
        Stat.CurrentStamina = Stat.MaxStamina;
        Stat.CurrentMoveSpeed = Stat.MoveSpeed;
        // GetAbility<PlayerUIAbility>().Refresh();
    }
}