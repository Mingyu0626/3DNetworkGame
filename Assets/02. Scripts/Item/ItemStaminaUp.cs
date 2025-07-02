using Photon.Pun;
using UnityEngine;

public class ItemStaminaUp : ItemObject
{
    protected override void ApplyItem(Player player)
    {
        player.Stat.CurrentStamina += Value;
        ItemObjectFactory.Instance.RequestDelete(photonView.ViewID);
    }
}
