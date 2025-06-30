using Photon.Pun;
using UnityEngine;

public class PlayerPresenter : MonoBehaviour
{
    private PlayerStat _playerStatModel;
    private UI_PlayerStatView _playerView;

    private PhotonView _photonView;

    private void Awake()
    {
        _playerStatModel = GetComponent<Player>().Stat;
        _playerView = FindFirstObjectByType<UI_PlayerStatView>();
        _photonView = GetComponent<PhotonView>();
    }

    private void Start()
    {
        if (!_photonView.IsMine)
        {
            return;
        }
        _playerStatModel.PlayerStaminaChanged += _playerView.SetSliderPlayerStamina;
        _playerView.InitSliderPlayerStamina(_playerStatModel.MaxStamina);

        _playerStatModel.PlayerHealthPointChanged += _playerView.SetSliderPlayerHealthPoint;
        _playerView.InitSliderPlayerHealthPoint(_playerStatModel.CurrentHealthPoint);
    }
}
