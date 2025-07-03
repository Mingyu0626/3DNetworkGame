using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviourPunCallbacks, IInRoomCallbacks
{
    public static ScoreManager Instance { get; private set; }
    private Dictionary<string, int> _scores = new Dictionary<string, int>();
    public Dictionary<string, int> Scores => _scores;

    private List<KeyValuePair<string, int>> _sortedScoreList;
    public List<KeyValuePair<string, int>> SortedScoreList => _sortedScoreList;

    public int _killCount;
    public int KillCount => _killCount;

    public event Action<List<KeyValuePair<string, int>>, string> OnDataChanged;

    // public List<UI_Score> UIScores;

    private void Awake()
    {
        Instance = this;
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
            nickname = myNickname;
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

    public void AddScore(int addedScore)
    {
        Hashtable hashtable = PhotonNetwork.LocalPlayer.CustomProperties;
        int currentScore = (int)hashtable["Score"];
        currentScore += addedScore;
        hashtable["Score"] = currentScore;
        PhotonNetwork.LocalPlayer.SetCustomProperties(hashtable);
    }

    public void AddKillCount()
    {
        _killCount++;
        Hashtable hashtable = PhotonNetwork.LocalPlayer.CustomProperties;
        int currentScore = (int)hashtable["Score"];
        currentScore += 5000;
        hashtable["Score"] = currentScore;
        PhotonNetwork.LocalPlayer.SetCustomProperties(hashtable);
    }

    public int GetMyScore()
    {
        string myNickName = $"{PhotonNetwork.NickName}_{PhotonNetwork.LocalPlayer.ActorNumber}";
        return _scores[myNickName];
    }
}
