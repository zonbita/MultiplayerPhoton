using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.IO;
using UnityEngine;

public class PlayerManager : MonoBehaviourPunCallbacks
{
    //PhotonView view;
    public float Health = 1f;

    private void Awake()
    {
        if (photonView.IsMine)
        {
            CreatePlayer();
        }
    }


    void CreatePlayer()
    {
        
        PhotonNetwork.Instantiate(Path.Combine("Prefabs", "PlayerController"), Vector3.zero, Quaternion.identity);
    }


    public override void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    public override void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }


    
}
