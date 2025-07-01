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
    private CharacterController _characterController;
    private PhotonView _photonView;
    private EPlayerState _state;
    public EPlayerState PlayerState => _state;
    public PhotonView PhotonView => _photonView;

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
        _state = EPlayerState.Live;

        _animator = GetComponent<Animator>();
        _characterController = GetComponent<CharacterController>();
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
    public void Damaged(float damage, int actorNumber)
    {
        Stat.CurrentHealthPoint -= damage;
        if (Stat.CurrentHealthPoint == 0 )
        {
            _state = EPlayerState.Death;
            StartCoroutine(Death_Coroutine());

            RoomManager.Instance.OnPlayerDeath(PhotonView.Owner.ActorNumber, actorNumber);
        }
        else
        {
            GetAbility<PlayerShakingAbility>().Shake();
        }
    }

    private IEnumerator Death_Coroutine()
    {
        _characterController.enabled = false;
        _photonView.RPC(nameof(PlayDeathAnimation), RpcTarget.All);

        yield return _deathTimer;

        _characterController.enabled = true;
        _photonView.RPC(nameof(PlayRespawnAnimation), RpcTarget.All);
        Respawn(PlayerSpawnManager.Instance.GetRandomSpawnPosition());
    }

    private void Respawn(Vector3 spawnPosition)
    {
        if (_photonView.IsMine)
        {
            transform.position = spawnPosition;
        }
        Stat.CurrentHealthPoint = Stat.MaxHealthPoint;
        Stat.CurrentStamina = Stat.MaxStamina;
        Stat.CurrentMoveSpeed = Stat.MoveSpeed;
        _state = EPlayerState.Live;
    }

    [PunRPC]
    private void PlayDeathAnimation()
    {
        _animator.SetTrigger($"Die");
    }

    [PunRPC]
    private void PlayRespawnAnimation()
    {
        _animator.SetTrigger($"Respawn");
    }
}