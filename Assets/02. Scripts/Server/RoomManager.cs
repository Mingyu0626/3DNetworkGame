using Photon.Pun;
using Photon.Realtime;
using TMPro;
using System;
using UnityEngine;

public class RoomManager : MonoBehaviourPunCallbacks
{
    private static RoomManager _instance;
    public static RoomManager Instance => _instance;

    private Room _room;
    public Room Room => _room;

    public event Action OnRoomDataChanged;
    public event Action<string> OnRoomLogChanged;

    public TextMeshProUGUI LogTextUI;
    private string _log;

    private void Awake()
    {
        _instance = this;
    }

    // "내가" 방에 입장하면 자동으로 호출되는 메서드
    public override void OnJoinedRoom()
    {
        GeneratePlayer();
        SetRoom();
        _log = $"\n<b><color=green>I</color></b> <color=blue>joined!</color>";
        OnRoomDataChanged?.Invoke();
        OnRoomLogChanged?.Invoke(_log);

    }

    // "다른 플레이어가" 방에 입장하면 자동으로 호출되는 메서드
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        _log = $"\n<b><color=green>{newPlayer.NickName}</color></b> <color=blue>joined!</color>";
        OnRoomDataChanged?.Invoke();
        OnRoomLogChanged?.Invoke(_log);
    }

    // "다른 플레이어가" 방에서 나가면 자동으로 호출되는 메서드
    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        _log = $"\n<b><color=green>{otherPlayer.NickName}</color></b> <color=blue>left.</color>";
        OnRoomDataChanged?.Invoke();
        OnRoomLogChanged?.Invoke(_log);
    }

    public void GeneratePlayer()
    {
        // 방에 입장했을 때, 플레이어를 생성한다.
        // 포톤에서는 게임 오브젝트 생성 후, 포톤 서버에 등록까지 해야한다.
        PhotonNetwork.Instantiate("Player",
            PlayerSpawnManager.Instance.GetRandomSpawnPosition(), Quaternion.identity);
    }

    private void SetRoom()
    {
        _room = PhotonNetwork.CurrentRoom;
        Debug.Log(_room.Name);
        Debug.Log(_room.PlayerCount);
        Debug.Log(_room.MaxPlayers);
    }
}
