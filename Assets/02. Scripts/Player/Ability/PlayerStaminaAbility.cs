using UnityEngine;
using Photon.Pun;

public class PlayerStaminaAbility : PlayerAbility
{
    private bool _isUsingStamina = false;
    public bool IsUsingStamina
    {
        get => _isUsingStamina;
        set => _isUsingStamina = value;
    }

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void DoAbility()
    {
        Recovery();
    }


    private void Recovery()
    {
        if (_isUsingStamina || Owner == null || Owner.Stat == null)
        {
            return;
        }
        Owner.Stat.CurrentStamina += Owner.Stat.StaminaRecoveryForSecond * Time.deltaTime;
    }
}