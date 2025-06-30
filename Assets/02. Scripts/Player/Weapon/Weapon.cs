using UnityEngine;

public class Weapon : MonoBehaviour
{
    private PlayerAttackAbility _playerAttackAbility;

    private void Start()
    {
        _playerAttackAbility = GetComponentInParent<PlayerAttackAbility>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"OnTriggerEnter 호출, other : {other.name}");
        if (other.transform == _playerAttackAbility.transform)
        {
            return;
        }

        IDamaged damagedObject = other.GetComponent<IDamaged>();
        if (damagedObject != null)
        {
            _playerAttackAbility.Hit(other);
        }
    }
}
