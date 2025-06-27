using Photon.Pun;
using UnityEngine;

public abstract class PlayerAbility : MonoBehaviour
{
    private Player _owner;
    protected Player Owner => _owner;

    private CharacterController _characterController;

    protected CharacterController CharacterController => _characterController;

    private Animator _animator;
    protected Animator Animator => _animator;

    private PhotonView _photonView;
    protected PhotonView PhotonView => _photonView;

    protected virtual void Awake()
    {
        _owner = GetComponent<Player>();
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        _photonView = GetComponent<PhotonView>();
    }

    protected virtual void Update()
    {
        if (!PhotonView.IsMine)
        {
            return;
        }
        DoAbility();
    }

    protected abstract void DoAbility();

}
