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
        Debug.Log("AttackState 진입");
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
        Debug.Log("AttackState 탈출");
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
                    Debug.Log("피격된 Player의 Damaged 메서드 호출");
                    _bossController.Animator.SetTrigger("Attack");
                    damaged.Damaged(_bossController.Stat.Damage, -1);
                }
                else
                {
                    Debug.Log("피격된 Player가 없음");
                }
            }
            yield return new WaitForSeconds(1f / _bossController.Stat.AttackSpeed);
        }
    }
}
