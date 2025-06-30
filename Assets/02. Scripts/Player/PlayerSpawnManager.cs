using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnManager : MonoBehaviourSingleton<PlayerSpawnManager>
{
    [SerializeField]
    private List<Transform> _randomSpawnPositions = new List<Transform>();

    protected override void Awake()
    {
        base.Awake();
    }

    public Vector3 GetRandomSpawnPosition()
    {
        return _randomSpawnPositions[Random.Range(0, _randomSpawnPositions.Count)].position;
    }
}
