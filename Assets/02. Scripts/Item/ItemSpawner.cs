using Photon.Pun;
using System.Collections;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public static ItemSpawner Instance { get; private set; }

    public float SpawnSpeed;
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
            Debug.Log("마스터 클라이언트이므로 코루틴 실행");
            _spawnDelayCache = new WaitForSeconds(SpawnSpeed);
            StartCoroutine(Spawn_Coroutine());
        }
        else
        {
            Debug.Log("마스터 클라이언트가 아니라고요?");
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
