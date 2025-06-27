using UnityEngine;

public class PlayerAbility : MonoBehaviour
{
    private Player _owner;
    protected Player Owner => _owner;

    private Animator _animator;
    protected Animator Animator => _animator;

    protected virtual void Awake()
    {
        _owner = GetComponent<Player>();
        _animator = GetComponent<Animator>();
    }
}
