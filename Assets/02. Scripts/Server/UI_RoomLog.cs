using TMPro;
using UnityEngine;

public class UI_RoomLog : MonoBehaviour
{
    public TextMeshProUGUI LogTextUI;
    private string _logMessages;

    private void Start()
    {
        RoomManager.Instance.OnRoomLogChanged += Refresh;
    }

    private void Refresh(string log)
    {
        _logMessages += log;
        LogTextUI.text = _logMessages;
    }
}
