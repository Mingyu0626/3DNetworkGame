using System.Collections.Generic;
using UnityEngine;

public class PlayerPositionManager : MonoBehaviourSingleton<PlayerPositionManager>
{
    private List<Player> _allPlayers = new List<Player>();
    protected override void Awake()
    {
        base.Awake();
    }

    public void AddPlayerToList(Player player)
    {
        _allPlayers.Add(player);
    }

    public Player GetNearestPlayer(Vector3 targetPosition)
    {
        float minDistance = float.MaxValue;
        Player nearestPlayer = null;
        foreach (var player in _allPlayers) 
        {
            float curDistance = Vector3.Distance(targetPosition, player.transform.position);
            if (curDistance < minDistance)
            {
                nearestPlayer = player;
                minDistance = curDistance;
            }
        }
        return nearestPlayer;
    }
}
