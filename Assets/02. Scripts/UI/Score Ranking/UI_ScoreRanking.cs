using Photon.Pun;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UI_ScoreRanking : MonoBehaviour
{
    public List<UI_ScoreSlot> Slots;
    public UI_ScoreSlot MySlot;
    private void Start()
    {
        ScoreManager.Instance.OnDataChanged += Refresh;
        ScoreManager.Instance.OnDataChanged += RefreshMySlot;
    }

    private void Refresh(List<KeyValuePair<string, int>> sortedScoreList, string myNickname)
    {
        for (int i = 0; i < Slots.Count; i++)
        {
            if (i < sortedScoreList.Count)
            {
                Slots[i].gameObject.SetActive(true);
                Slots[i].Refresh($"{i + 1}", sortedScoreList[i].Key, sortedScoreList[i].Value);
            }
            else
            {
                Slots[i].gameObject.SetActive(false);
            }
        }

    }

    private void RefreshMySlot(List<KeyValuePair<string, int>> sortedScoreList, string myNickname)
    {
        int index = sortedScoreList.FindIndex(x => x.Key == myNickname);
        MySlot.Refresh($"{index + 1}", sortedScoreList[index].Key, sortedScoreList[index].Value);
    }
}
