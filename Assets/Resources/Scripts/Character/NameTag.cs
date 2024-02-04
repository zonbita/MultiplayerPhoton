using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class NameTag : MonoBehaviourPunCallbacks
{
    [HideInInspector]
    public Transform target = null;

    [SerializeField]
    private Text nameText;

    PhotonView pv;
    private void Awake()
    {
        pv = GetComponentInParent<PhotonView>();
    }
    void Start()
    {
        if (pv.IsMine)
        {
            pv.RPC("RPC_SetName", RpcTarget.All, PhotonNetwork.NickName);
            nameText.text = PhotonNetwork.NickName;
        }
        else
        {
            nameText.text = pv.Owner.NickName;
        }
    
    }

    [PunRPC]
    public void RPC_SetName(string name)
    {
        nameText.text = name;
    }
}
