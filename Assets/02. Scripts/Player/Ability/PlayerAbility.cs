using UnityEngine;

public class PlayerAbility : MonoBehaviour
{
    private Player _owner;
    public Player Owner => _owner;

    private Animator _animator;
    public Animator Animator => _animator;

    protected virtual void Awake()
    {
        _owner = GetComponent<Player>();
        _animator = GetComponent<Animator>();
    }
}
