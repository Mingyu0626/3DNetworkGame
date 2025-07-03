using UnityEngine;

public class BossTraceState : IBossState
{
    private BossController _bossController;

    private float _contextToPatrolTimer;
    public BossTraceState(BossController bossController)
    {
        _bossController = bossController;
    }

    public void Enter()
    {
        _contextToPatrolTimer = 0f;
    }

    public void Update()
    {
        float distance = Vector3.Distance(_bossController.Player.transform.position, _bossController.transform.position);
        if (_bossController.Stat.DistanceForTrace < distance)
        {
            _contextToPatrolTimer += Time.deltaTime;
            if (_bossController.Stat.ContextToPatrolTime <= _contextToPatrolTimer)
            {
                _bossController.BossStateContext.ChangeState(_bossController.BossStateDict[EBossState.Patrol]);
            }
        }
        else if (distance <= _bossController.Stat.DistanceForAttack)
        {
            _bossController.BossStateContext.ChangeState(_bossController.BossStateDict[EBossState.Attack]);
        }
        else
        {
            _contextToPatrolTimer = 0f;
            _bossController.NavmeshAgent.SetDestination(_bossController.Player.transform.position);
            //_bossController.transform.position
            //    = Vector3.MoveTowards
            //    (
            //        _bossController.transform.position,
            //    _bossController.Player.transform.position,
            //    _bossController.Stat.MoveSpeed * Time.deltaTime
            //    );
            // RotateTowardsPlayer();
        }
    }

    public void Exit()
    {

    }

    private void RotateTowardsPlayer()
    {
        Vector3 directionToPlayer = _bossController.Player.transform.position - _bossController.transform.position;
        directionToPlayer.y = 0;

        if (directionToPlayer == Vector3.zero)
        {
            return;
        }

        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
        _bossController.transform.rotation = Quaternion.Slerp(
            _bossController.transform.rotation,
            targetRotation,
            _bossController.Stat.RotateSpeed * Time.deltaTime 
        );
    }
}
