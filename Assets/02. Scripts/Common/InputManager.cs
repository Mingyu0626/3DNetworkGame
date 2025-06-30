using UnityEngine;

public class InputManager : MonoBehaviourSingleton<InputManager>
{
    private bool _isInputBlocked;
    public bool IsInputBlocked
    {
        get => _isInputBlocked;
        set => _isInputBlocked = value;
    }
    protected override void Awake()
    {
        base.Awake();
    }
}
