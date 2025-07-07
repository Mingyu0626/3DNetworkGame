using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class LobbyScene : MonoBehaviour
{
    public TMP_InputField NickinameInputField;
    public TMP_InputField RoomNameInputField;

    // 현업에서 싱글톤 상속 계층구조 설계방식
    // (1). 종류별로 따로 기본 스크립트를 만든다.
    // 1. C# 전용
    // 2. DontDestroyOnLoad(false) - 
    // 3. DontDestroyOnLoad(true) - PermanantSingletonBehaviour

    // (2). 위 내용을 옵션으로 선택할 수 있게끔 한다.
    // - 또한, 위 내용 뿐만이 아니라 초기화 시점도 선택 가능하게끔 한다.


    // 방 만들기 함수를 호출
    public void OnClickMakeRoomButton()
    {
        MakeRoom();
    }

    private void MakeRoom()
    {
        string nickname = NickinameInputField.text;
        string roomName = RoomNameInputField.text;

        if (string.IsNullOrEmpty(nickname) || string.IsNullOrEmpty(roomName))
        {
            return;
        }

        // 포톤에 닉네임 등록
        PhotonNetwork.NickName = nickname;

        // Room 속성 정의
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 20;
        roomOptions.IsOpen = true;     // 룸 입장 가능 여부
        roomOptions.IsVisible = true;  // 로비(채널) 룸 목록에 노출시킬지 여부

        // Room 생성
        PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, TypedLobby.Default);
    }
}