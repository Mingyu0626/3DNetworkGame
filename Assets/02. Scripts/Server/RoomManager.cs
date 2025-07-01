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
    public event Action<string> OnPlayerEntered;
    public event Action<string> OnPlayerExit;
    public event Action<string, string> OnPlayerDead;


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
        OnRoomDataChanged?.Invoke();
    }

    // "다른 플레이어가" 방에 입장하면 자동으로 호출되는 메서드
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        OnRoomDataChanged?.Invoke();
        OnPlayerEntered?.Invoke($"{newPlayer.NickName}_{newPlayer.ActorNumber}");
    }

    // "다른 플레이어가" 방에서 나가면 자동으로 호출되는 메서드
    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        OnRoomDataChanged?.Invoke();
        OnPlayerExit?.Invoke($"{otherPlayer.NickName}_{otherPlayer.ActorNumber}");
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

    public void OnPlayerDeath(int actorNumber, int otherActorNumber)
    {
        // actorNumber가 otherActorNumber에 의해 죽었다.
        string deadPlayerName
            = PhotonNetwork.CurrentRoom.GetPlayer(actorNumber).NickName;
        string killerPlayerName
            = PhotonNetwork.CurrentRoom.GetPlayer(otherActorNumber).NickName;
        OnPlayerDead?.Invoke
            ($"{deadPlayerName}_{actorNumber}", $"{killerPlayerName}_{otherActorNumber}");
    }
}
