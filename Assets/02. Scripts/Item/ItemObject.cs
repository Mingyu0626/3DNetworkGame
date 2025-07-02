using Photon.Pun;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class ItemObject : MonoBehaviour
{
    [Header("아이템 타입")]
    public EItemType ItemType;
    public float Value = 100;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            player.Score += 10;
            Destroy(gameObject);
        }
    }
}
