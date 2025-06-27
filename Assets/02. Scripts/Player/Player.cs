using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerStat Stat;
    private List<PlayerAbility> _abilities = new List<PlayerAbility>();

    private void Awake()
    {
        _abilities = GetComponents<PlayerAbility>().ToList();
    }

    public T GetAbility<T>() where T : PlayerAbility
    {
        return _abilities.OfType<T>().FirstOrDefault();
    }
}
