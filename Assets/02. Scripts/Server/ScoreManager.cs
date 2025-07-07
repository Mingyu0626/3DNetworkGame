using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[RequireComponent(typeof(PhotonView))]
public class ScoreManager : MonoBehaviourPunCallbacks, IInRoomCallbacks
{
    public static ScoreManager Instance { get; private set; }

    private Dictionary<string, int> _scores = new Dictionary<string, int>();



    public Dictionary<string, int> Scores => _scores;

    private List<KeyValuePair<string, int>> _sortedScoreList;
    public List<KeyValuePair<string, int>> SortedScoreList => _sortedScoreList;

    public int _killCount;

    public event Action<List<KeyValuePair<string, int>>, string> OnDataChanged;
    public event Action<int> OnScoreChanged;


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        // 방에 들어가면 '내 점수가 0이다' 라는 내용으로 
        // 커스텀 프로퍼티를 초기화해준다.
        if (PhotonNetwork.InRoom)
        {
            Refresh();
        }
    }

    public override void OnJoinedRoom()
    {
        Refresh();
    }

    // 플레이어의 커스텀 프로퍼티가 변경되면 호출되는 콜백 함수
    public override void OnPlayerPropertiesUpdate(Photon.Realtime.Player targetPlayer, Hashtable changedProps)
    {
        UpdateScoreDictionary();
        UpdateSortedList();

        int index = 0;
        string myNickname;
        if (IsMyNicknameInScoreList(out index, out myNickname))
        {
            OnDataChanged?.Invoke(_sortedScoreList, myNickname);
            OnScoreChanged?.Invoke(GetMyScore());
        }
    }

    private void UpdateScoreDictionary()
    {
        var roomPlayers = PhotonNetwork.PlayerList;
        foreach (var player in roomPlayers)
        {
            if (player.CustomProperties.ContainsKey("Score"))
            {
                _scores[$"{player.NickName}_{player.ActorNumber}"] = (int)player.CustomProperties["Score"];
            }
        }
    }

    private void UpdateSortedList()
    {
        _sortedScoreList = _scores.ToList().OrderByDescending(x => x.Value).ToList();
    }

    private bool IsMyNicknameInScoreList(out int index, out string nickname)
    {
        string myNickname = $"{PhotonNetwork.NickName}_{PhotonNetwork.LocalPlayer.ActorNumber}";
        index = _sortedScoreList.FindIndex(x => x.Key == myNickname);
        nickname = myNickname;
        if (0 <= index)
        {
            return true;
        }
        return false;
    }

    public void Refresh()
    {
        Hashtable hashtable = new Hashtable();
        hashtable.Add("Score", 0);
        PhotonNetwork.LocalPlayer.SetCustomProperties(hashtable);
    }

    [PunRPC]
    public void IncreaseScore(int increasedScore)
    {
        Hashtable hashtable = PhotonNetwork.LocalPlayer.CustomProperties;
        int currentScore = (int)hashtable["Score"];
        currentScore += increasedScore;
        hashtable["Score"] = currentScore;
        PhotonNetwork.LocalPlayer.SetCustomProperties(hashtable);
    }

    [PunRPC]
    public void DecreaseScore(int decreasedScore)
    {
        Hashtable hashtable = PhotonNetwork.LocalPlayer.CustomProperties;
        int currentScore = (int)hashtable["Score"];
        currentScore -= decreasedScore;
        hashtable["Score"] = currentScore;
        PhotonNetwork.LocalPlayer.SetCustomProperties(hashtable);
    }

    public void AddKillCount()
    {
        _killCount++;
        IncreaseScore(5000);
    }

    public void StealDeadPlayerScore(int deadActorNumber)
    {
        Debug.Log("StealDeadPlayerScore");
        Photon.Realtime.Player deadPlayer = PhotonNetwork.CurrentRoom.GetPlayer(deadActorNumber);
        int halfScore = GetPlayerScore(deadActorNumber) / 2;

        IncreaseScore(halfScore);
        photonView.RPC("DecreaseScore", deadPlayer, halfScore);
    }

    public int GetMyScore()
    {
        string myNickName = $"{PhotonNetwork.NickName}_{PhotonNetwork.LocalPlayer.ActorNumber}";
        return _scores[myNickName];
    }

    public int GetPlayerScore(int actorNumber)
    {
        Photon.Realtime.Player player = PhotonNetwork.CurrentRoom.GetPlayer(actorNumber);
        string playerNickName = $"{player.NickName}_{player.ActorNumber}";
        return _scores[playerNickName];
    }
}
