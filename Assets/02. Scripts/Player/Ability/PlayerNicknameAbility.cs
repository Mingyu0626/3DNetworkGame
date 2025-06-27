using TMPro;
using UnityEngine;

public class PlayerNicknameAbility : PlayerAbility
{
    [SerializeField]
    private TextMeshProUGUI _nicknameTextUI;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        _nicknameTextUI.text = PhotonView.Owner.NickName;

        if (PhotonView.IsMine)
        {
            _nicknameTextUI.color = Color.green;
        }
        else
        {
            _nicknameTextUI.color = Color.red;
        }

    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void DoAbility()
    {
    }
}
