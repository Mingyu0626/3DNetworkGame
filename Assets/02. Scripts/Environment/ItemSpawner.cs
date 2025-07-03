using Photon.Pun;
using System.Collections;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public static ItemSpawner Instance { get; private set; }

    public float Interval;
    public float RandomX;
    public float RandomZ;
    private WaitForSeconds _spawnDelayCache;

    private void Awake()
    {
        Instance = this;
    }

    public void StartSpawnCoroutine()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            _spawnDelayCache = new WaitForSeconds(Interval);
            StartCoroutine(Spawn_Coroutine());
        }
    }

    private IEnumerator Spawn_Coroutine()
    {
        while (true)
        {
            Vector3 randomPosition = 
                new Vector3
                (Random.Range(-RandomX, RandomX), 0f, Random.Range(-RandomZ, RandomZ));
            ItemObjectFactory.Instance.RequestCreate(EItemType.Item_ScoreUp, randomPosition);
            yield return _spawnDelayCache;
        }
    }
}
