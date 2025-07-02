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
            _bossController.transform.position
                = Vector3.MoveTowards
                (
                    _bossController.transform.position,
                _bossController.Player.transform.position,
                _bossController.Stat.MoveSpeed * Time.deltaTime
                );
        }
    }

    public void Exit()
    {

    }
}
