using Photon.Pun;
using UnityEngine;

public class ItemScoreUp : ItemObject
{
    protected override void ApplyItem(Player player)
    {
        ScoreManager.Instance.IncreaseScore(Value);
        ItemObjectFactory.Instance.RequestDelete(photonView.ViewID);
    }
}
