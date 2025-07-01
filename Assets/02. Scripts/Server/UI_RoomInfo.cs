using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class UI_RoomInfo : MonoBehaviourPunCallbacks
{
    public TextMeshProUGUI RoomNameTextUI;
    public TextMeshProUGUI PlayerCountTextUI;

    private void Start()
    {
        RoomManager.Instance.OnRoomDataChanged += Refresh;
        Refresh();
    }
    private void Refresh()
    {
        Room room = RoomManager.Instance.Room;
        if (room == null)
        {
            return;
        }

        RoomNameTextUI.text = room.Name;
        PlayerCountTextUI.text = $"{room.PlayerCount}/{room.MaxPlayers}";

        // LogTextUI.text = _logText;
        // PlayerCountTextUI.text = $"{PhotonNetwork.CurrentRoom.PlayerCount}/{PhotonNetwork.CurrentRoom.MaxPlayers}";
    }

    public void OnClickExitButton()
    {
        Exit();
    }

    private void Exit()
    {
        
    }
}