using Photon.Realtime;
using UnityEngine;

public class BossPatrolState : IBossState
{
    private BossController _bossController;

    private int _currentPatrolIndex = -1;
    private const float REACH_DISTANCE_THRESHOLD = 1f;
    public BossPatrolState(BossController bossController)
    {
        _bossController = bossController;
    }

    public void Enter()
    {
        _bossController.transform.position = SpawnPositionManager.Instance.GetRandomSpawnPosition();
        _currentPatrolIndex = GetNearestPatrolPositionIndex();
    }

    public void Update()
    {
        _bossController.Player = PlayerPositionManager.Instance.GetNearestPlayer(_bossController.transform.position);
        float distance = Vector3.Distance(_bossController.Player.transform.position, _bossController.transform.position);
        if (distance <= _bossController.Stat.DistanceForTrace)
        {
            _bossController.BossStateContext.ChangeState(_bossController.BossStateDict[EBossState.Trace]);
        }
        else
        {
            Patrol();
        }
    }

    public void Exit()
    {
    }

    private int GetNearestPatrolPositionIndex()
    {
        float minDistance = float.MaxValue;
        int nearestPatrolPositionIndex = -1;
        int curIndex = 0;
        foreach (var patrolPosition in _bossController.PatrolPositions)
        {
            float curDistance = Vector3.Distance(_bossController.transform.position, patrolPosition);
            if (curDistance < minDistance)
            {
                nearestPatrolPositionIndex = curIndex;
                minDistance = curDistance;
            }
            curIndex++;
        }
        return nearestPatrolPositionIndex;
    }

    private void Patrol()
    {
        if (_bossController.PatrolPositions.Count == 0 || _currentPatrolIndex == -1)
        {
            return;
        }

        Vector3 currentPosition = _bossController.transform.position;
        Vector3 targetPosition = _bossController.PatrolPositions[_currentPatrolIndex];
        float distanceToTarget = Vector3.Distance(currentPosition, targetPosition);
        if (distanceToTarget <= REACH_DISTANCE_THRESHOLD)
        {
            _currentPatrolIndex = (_currentPatrolIndex + 1) % _bossController.PatrolPositions.Count;
        }

        _bossController.transform.position = 
            Vector3.MoveTowards(
                currentPosition, 
                targetPosition, 
                _bossController.Stat.MoveSpeed * Time.deltaTime);

        RotateTowardsPatrolPosition();
    }

    private void RotateTowardsPatrolPosition()
    {
        Vector3 directionToPlayer = _bossController.PatrolPositions[_currentPatrolIndex] - _bossController.transform.position;
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
