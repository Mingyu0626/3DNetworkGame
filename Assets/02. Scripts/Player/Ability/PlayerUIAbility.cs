using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class PlayerUIAbility : PlayerAbility
{
    [SerializeField]
    private TextMeshProUGUI _nicknameTextUI;
    [SerializeField]
    private Slider _playerHealthPointBarOnPlayer;

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
            _playerHealthPointBarOnPlayer.fillRect.gameObject.GetComponent<Image>().color = Color.green;
        }
        else
        {
            _nicknameTextUI.color = Color.red;
            _playerHealthPointBarOnPlayer.fillRect.gameObject.GetComponent<Image>().color = Color.red;
        }
        _playerHealthPointBarOnPlayer.maxValue = Owner.Stat.MaxHealthPoint;
        _playerHealthPointBarOnPlayer.value = Owner.Stat.MaxHealthPoint;
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void DoAbility()
    {
    }

    public void Refresh()
    {
        _playerHealthPointBarOnPlayer.value = Owner.Stat.CurrentHealthPoint;
    }
}
