using Photon.Pun;
using UnityEngine;

public abstract class ItemObject : MonoBehaviour
{
    [Header("아이템 타입")]
    public EItemType ItemType;
    public int Value = 100;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                ApplyItem(player);
            }
        }
    }


    protected abstract void ApplyItem(Player player);

}
