using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class UI_PlayerStatView : MonoBehaviour
{
    [SerializeField]
    private Slider _playerStaminaBar;
    [SerializeField]
    private Slider _playerHealthPointBar;

    public void InitSliderPlayerStamina(float maxStamina)
    {
        if (!ReferenceEquals(_playerStaminaBar, null))
        {
            _playerStaminaBar.maxValue = maxStamina;
            _playerStaminaBar.value = maxStamina;
        }
    }
    public void InitSliderPlayerHealthPoint(float maxHealthPoint)
    {
        if (!ReferenceEquals(_playerHealthPointBar, null))
        {
            _playerHealthPointBar.maxValue = maxHealthPoint;
            _playerHealthPointBar.value = maxHealthPoint;
        }
    }

    public void SetSliderPlayerStamina(float stamina)
    {


        if (!ReferenceEquals(_playerStaminaBar, null))
        {
            _playerStaminaBar.value = stamina;
        }
    }
    public void SetSliderPlayerHealthPoint(float healthPoint)
    {


        if (!ReferenceEquals(_playerHealthPointBar, null))
        {
            _playerHealthPointBar.value = healthPoint;
        }
    }
}
