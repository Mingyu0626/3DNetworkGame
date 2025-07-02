using UnityEngine;

public class BossDamagedState : IBossState
{
    private BossController _bossController;
    public BossDamagedState(BossController bossController)
    {
        _bossController = bossController;
    }
    public void Enter()
    {
        _bossController.Animator.SetTrigger("Hit");
    }

    public void Update()
    {
        _bossController.BossStateContext.ChangeState(_bossController.BossStateDict[EBossState.Trace]);
    }

    public void Exit()
    {

    }
}
