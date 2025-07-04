using TMPro;
using UnityEngine;

public class UI_RoomLog : MonoBehaviour
{
    public TextMeshProUGUI LogTextUI;
    private string _logMessages;

    private void Start()
    {
        RoomManager.Instance.OnPlayerEntered += PlayerEnterLog;
        RoomManager.Instance.OnPlayerExit += PlayerExitLog;
        RoomManager.Instance.OnPlayerDead += PlayerDeathLog;
        Refresh();
    }

    private void Refresh()
    {
        LogTextUI.text = _logMessages;
    }

    public void PlayerEnterLog(string playerName)
    {
        _logMessages += 
            $"\n<b><color=green>{playerName}</color></b> <color=blue>joined!</color>";
        Refresh();
    }

    public void PlayerExitLog(string playerName)
    {
        _logMessages +=
            $"\n<b><color=green>{playerName}</color></b> <color=blue>exit.</color>";
        Refresh();
    }

    public void PlayerDeathLog(string deadPlayerName, string killerName)
    {
        _logMessages +=
            $"\n<b><color=red>{killerName}</color></b> killed <color=purple>{deadPlayerName}!</color>";
        Refresh();
    }
}
