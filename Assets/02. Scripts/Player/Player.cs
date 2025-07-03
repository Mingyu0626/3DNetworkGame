using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
[RequireComponent(typeof(PhotonTransformView))]
[RequireComponent(typeof(PhotonAnimatorView))]
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

    public GameObject Weapon;
    private int _weaponScaleStack;
    private const int _weaponScaleUpFactor = 10000;

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

        PlayerPositionManager.Instance.AddPlayerToList(this);
        ItemSpawner.Instance.StartSpawnCoroutine();
        ScoreManager.Instance.OnScoreChanged += TryRaiseWeaponSize;
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
        if (Stat.CurrentHealthPoint == 0)
        {
            _state = EPlayerState.Death;
            StartCoroutine(Death_Coroutine());
            RoomManager.Instance.OnPlayerDeath(PhotonView.Owner.ActorNumber, actorNumber);


            OnDead();
            // 나를 죽인 플레이어의 OnKill 함수 RPC 호출
            Photon.Realtime.Player lastHitPlayer = PhotonNetwork.CurrentRoom.GetPlayer(actorNumber);
            _photonView.RPC("OnKill", lastHitPlayer, PhotonView.Owner.ActorNumber);

            if (_photonView.IsMine)
            {
                MakeRandomItems(UnityEngine.Random.Range(1, 3));
            }
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
        Respawn(SpawnPositionManager.Instance.GetRandomSpawnPosition());
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

    private void MakeRandomItems(int count)
    {
        for (int i = 0; i < count; i++)
        {
            // 포톤의 네트워크 객체의 생성 주기
            // Player : 플레이어가 생성하고, 플레이어가 나가면 자동 삭제(PhotonNetwork.Instantiate)
            // Room : "방장만" 생성하고, 룸이 없어지면 삭제(PhotonNetwork.InstantiateRoomObject)
            ItemObjectFactory.Instance.RequestCreate
                ((EItemType)UnityEngine.Random.Range(0, (int)EItemType.Count), 
                transform.position + new Vector3(0f, 2f, 0f));
        }
    }

    [PunRPC]
    public void OnKill(int deadActorNumber)
    {
        Debug.Log("킬을 달성한 플레이어 측에서 OnKill 호출");
        ScoreManager.Instance.AddKillCount();
        ScoreManager.Instance.StealDeadPlayerScore(deadActorNumber);
    }

    private void OnDead()
    {

    }


    private void TryRaiseWeaponSize(int score)
    {
        int newScaleStack = score / _weaponScaleUpFactor;
        if (_weaponScaleStack == newScaleStack)
        {
            return;
        }
        _weaponScaleStack = newScaleStack;
        float newScale = 1f + _weaponScaleStack * 0.1f;
        Weapon.transform.localScale = new Vector3(newScale, newScale, newScale);
    }
}