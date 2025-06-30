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

    [PunRPC]
    public void Refresh()
    {
        _playerHealthPointBarOnPlayer.value = Owner.Stat.CurrentHealthPoint;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // 데이터를 전송하는 상황 -> 데이터를 보내주면 되고
        if (stream.IsWriting)
        {
            stream.SendNext(Owner.Stat.CurrentHealthPoint);
            Refresh();
        }

        // 데이터를 수신하는 상황 -> 받은 데이터를 세팅하면 된다.
        else if (stream.IsReading)
        {
            Owner.Stat.CurrentHealthPoint = (float)stream.ReceiveNext();
            Refresh();
        }
    }
}
