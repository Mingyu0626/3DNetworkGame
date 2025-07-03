using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class BossController : MonoBehaviour, IDamaged
{
    [Header("State System")]
    private BossStateContext _bossStateContext;
    public BossStateContext BossStateContext => _bossStateContext;

    private Dictionary<EBossState, IBossState> _bossStateDict = new Dictionary<EBossState, IBossState>();
    public Dictionary<EBossState, IBossState> BossStateDict { get => _bossStateDict; set => _bossStateDict = value; }

    [Header("Components")]
    private Collider _collider;
    public Collider Collider => _collider;

    private Animator _animator;
    public Animator Animator { get => _animator; set => _animator = value; }

    private NavMeshAgent _navMeshAgent;
    public NavMeshAgent NavmeshAgent => _navMeshAgent;

    [Header("References")]
    private Player _player;
    public Player Player { get => _player; set => _player = value; }

    public BossStat Stat;

    private List<Vector3> _patrolPositions = new List<Vector3>();
    public List<Vector3> PatrolPositions => _patrolPositions;


    private void Awake()
    {
        if (Stat == null)
        {
            Stat = new BossStat();
        }
        _bossStateContext = new BossStateContext(this);
        _bossStateDict = new Dictionary<EBossState, IBossState>();
        _bossStateDict.Add(EBossState.Patrol, new BossPatrolState(this));
        _bossStateDict.Add(EBossState.Trace, new BossTraceState(this));
        _bossStateDict.Add(EBossState.Attack, new BossAttackState(this));
        _bossStateDict.Add(EBossState.Damaged, new BossDamagedState(this));
        _bossStateDict.Add(EBossState.Die, new BossDieState(this));


        _collider = GetComponent<CharacterController>();
        _animator = GetComponentInChildren<Animator>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.speed = Stat.MoveSpeed;
        SetPatrolPositions();
    }

    private void OnEnable()
    {
        Stat.CurrentHealthPoint = Stat.MaxHealthPoint;
        _bossStateContext.ChangeState(_bossStateDict[EBossState.Patrol]);
    }

    private void Update()
    {
        _bossStateContext.CurrentState.Update();
    }

    public void StartCoroutineInBossState(IEnumerator coroutine)
    {
        StartCoroutine(coroutine);
    }

    public void StopCoroutineInBossState(IEnumerator coroutine)
    {
        StopCoroutine(coroutine);
    }

    public void Damaged(float damage, int actorNumber)
    {
        Stat.CurrentHealthPoint -= damage;
        if (Stat.CurrentHealthPoint <= 0f)
        {
            _bossStateContext.ChangeState(_bossStateDict[EBossState.Die]);
        }
        else
        {
            _bossStateContext.ChangeState(_bossStateDict[EBossState.Damaged]);
        }
    }

    private void SetPatrolPositions()
    {
        _patrolPositions.Clear();
        GameObject[] patrolPoints = GameObject.FindGameObjectsWithTag("BossPatrolPosition");
        foreach (GameObject point in patrolPoints)
        {
            _patrolPositions.Add(point.transform.position);
        }
    }
}
