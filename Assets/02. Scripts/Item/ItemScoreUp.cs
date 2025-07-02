using Photon.Pun;
using UnityEngine;

public class ItemScoreUp : ItemObject
{
    protected override void ApplyItem(Player player)
    {
        player.Score += Value;
        PhotonNetwork.Destroy(gameObject);
    }
}
