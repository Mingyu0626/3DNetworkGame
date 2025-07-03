using Photon.Pun;
using UnityEngine;

public class ItemScoreUp : ItemObject
{
    protected override void ApplyItem(Player player)
    {
        ScoreManager.Instance.AddScore(Value);
        ItemObjectFactory.Instance.RequestDelete(photonView.ViewID);
    }
}
