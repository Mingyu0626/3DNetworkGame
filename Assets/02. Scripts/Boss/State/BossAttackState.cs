using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackState : IBossState
{
    private BossController _bossController;
    private IEnumerator _attackCoroutine;
    private int _playerLayer;


    public BossAttackState(BossController bossController)
    {
        _bossController = bossController;
        _playerLayer = LayerMask.GetMask("Player");
    }
    public void Enter()
    {
        _attackCoroutine = AttackCoroutine();
        _bossController.StartCoroutineInBossState(_attackCoroutine);
    }

    public void Update()
    {
        if (_bossController.Stat.DistanceForAttack <
            Vector3.Distance(_bossController.Player.transform.position,
            _bossController.transform.position))
        {
            _bossController.BossStateContext.ChangeState(_bossController.BossStateDict[EBossState.Trace]);

        }
    }

    public void Exit()
    {
        if (!ReferenceEquals(_attackCoroutine, null))
        {
            _bossController.StopCoroutineInBossState(_attackCoroutine);
            _attackCoroutine = null;
        }

    }

    private IEnumerator AttackCoroutine()
    {
        while (true)
        {
            Collider[] hitColliders = Physics.OverlapSphere
                (_bossController.transform.position, _bossController.Stat.AttackRange, _playerLayer);
            foreach (Collider collider in hitColliders)
            {
                if (collider.TryGetComponent<IDamaged>(out IDamaged damaged))
                {
                    _bossController.Animator.SetTrigger("Attack");
                    damaged.Damaged(_bossController.Stat.Damage, -1);
                }
                else
                {
                    Debug.Log("Player is not in attack range");
                }
            }
            yield return new WaitForSeconds(1f / _bossController.Stat.AttackSpeed);
        }
    }
}
