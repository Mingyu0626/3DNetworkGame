using UnityEngine;

public class PlayerPresenter : MonoBehaviour
{
    private PlayerStat _playerStatModel;
    private UI_PlayerStatView _playerView;

    private void Awake()
    {
        _playerStatModel = GetComponent<Player>().Stat;
        _playerView = FindFirstObjectByType<UI_PlayerStatView>();
    }

    private void Start()
    {
        _playerStatModel.PlayerStaminaChanged += _playerView.SetSliderPlayerStamina;
        _playerView.InitSliderPlayerStamina(_playerStatModel.MaxStamina);

        _playerStatModel.PlayerHealthPointChanged += _playerView.SetSliderPlayerHealthPoint;
        _playerView.InitSliderPlayerHealthPoint(_playerStatModel.CurrentHealthPoint);
    }
}
