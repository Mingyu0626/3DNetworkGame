using Photon.Pun;
using UnityEngine;

public class ItemHealthUp : ItemObject
{
    protected override void ApplyItem(Player player)
    {
        player.Stat.CurrentHealthPoint += Value;
        ItemObjectFactory.Instance.RequestDelete(GetComponent<PhotonView>().ViewID);
    }
}
