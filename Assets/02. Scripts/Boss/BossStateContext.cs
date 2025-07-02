using UnityEngine;

public class BossStateContext
{
    private IBossState _currentState;
    public IBossState CurrentState { get => _currentState; set => _currentState = value; }
    private BossController _bossController;

    public BossStateContext(BossController controller)
    {
        _bossController = controller;
    }

    public void ChangeState()
    {
        _currentState = new BossPatrolState(_bossController);
        _currentState.Enter();
    }

    public void ChangeState(IBossState newState)
    {
        if (!ReferenceEquals(_currentState, null))
        {
            _currentState.Exit();
        }
        _currentState = newState;
        _currentState.Enter();
    }
}
