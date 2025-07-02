using System.Threading;
using UnityEngine;

public class BossDieState : IBossState
{
    private BossController _bossController;
    private float _respawnTimer;

    
    public BossDieState(BossController bossController)
    {
        _bossController = bossController;
        _respawnTimer = 0f;
    }
    public void Enter()
    {
        _bossController.Collider.enabled = false;
        _bossController.Animator.SetBool("Dead", true);
    }

    public void Update()
    {
        _respawnTimer += Time.deltaTime;
        if (_bossController.Stat.RespawnTime <= _respawnTimer)
        {
            _bossController.BossStateContext.ChangeState(_bossController.BossStateDict[EBossState.Patrol]);
        }
    }

    public void Exit()
    {
        _bossController.Collider.enabled = true;
        _bossController.Animator.SetBool("Dead", false);
    }
}
