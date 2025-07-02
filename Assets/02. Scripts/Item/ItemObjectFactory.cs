using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class ItemObjectFactory : MonoBehaviourPun
{
    public static ItemObjectFactory Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void RequestCreate(EItemType type, Vector3 position)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Create(type, position);
        }
        else
        {
            photonView.RPC(nameof(Create), RpcTarget.MasterClient, type, position);
        }
    }

    [PunRPC]
    private void Create(EItemType type, Vector3 position)
    {
        Vector3 dropPos = position + new Vector3(0, 0.5f, 0f) + UnityEngine.Random.insideUnitSphere;
        PhotonNetwork.InstantiateRoomObject(type.ToString(), dropPos, Quaternion.identity);
    }

    public void RequestDelete(int viewID)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("MasterClient의 Delete");
            Delete(viewID);
        }
        else
        {
            Debug.Log("Client의 Delete");
            photonView.RPC(nameof(Delete), RpcTarget.MasterClient, viewID);
        }
    }

    [PunRPC]
    private void Delete(int viewID)
    {
        PhotonView photonView = PhotonView.Find(viewID);
        if (photonView != null)
        {
            Debug.Log($"{viewID}에 해당하는 go 발견, 삭제 진행");
            PhotonNetwork.Destroy(photonView.gameObject);
        }
    }
}
