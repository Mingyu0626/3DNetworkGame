using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIAbility : PlayerAbility, IPunObservable
{
    [SerializeField]
    private TextMeshProUGUI _nicknameTextUI;
    [SerializeField]
    private Slider _playerHealthPointBarOnPlayer;

    private float _receviedHeathPoint;

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
        Refresh();
    }

    protected override void DoAbility()
    {
    }

    public void Refresh()
    {
        if (PhotonView.IsMine)
        {
            _playerHealthPointBarOnPlayer.value = Owner.Stat.CurrentHealthPoint;

        }
        else
        {
            _playerHealthPointBarOnPlayer.value = _receviedHeathPoint;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // 데이터를 전송하는 상황 -> 데이터를 보내주면 되고
        if (stream.IsWriting)
        {
            stream.SendNext(Owner.Stat.CurrentHealthPoint);
        }

        // 데이터를 수신하는 상황 -> 받은 데이터를 세팅하면 된다.
        else if (stream.IsReading)
        {
            _receviedHeathPoint = (float)stream.ReceiveNext();
        }
    }
}
