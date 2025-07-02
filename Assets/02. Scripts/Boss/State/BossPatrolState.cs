using UnityEngine;

public class BossPatrolState : IBossState
{
    private BossController _bossController;
    public BossPatrolState(BossController bossController)
    {
        _bossController = bossController;
    }

    public void Enter()
    {
        _bossController.transform.position = SpawnPositionManager.Instance.GetRandomSpawnPosition();
    }

    public void Update()
    {
        _bossController.Player = PlayerPositionManager.Instance.GetNearestPlayer(_bossController.transform.position);
        float distance = Vector3.Distance(_bossController.Player.transform.position, _bossController.transform.position);
        if (distance <= _bossController.Stat.DistanceForTrace)
        {
            _bossController.BossStateContext.ChangeState(_bossController.BossStateDict[EBossState.Trace]);
        }
    }

    public void Exit()
    {
    }
}
