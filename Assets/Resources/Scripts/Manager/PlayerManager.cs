using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.IO;
using UnityEngine;

public class PlayerManager : MonoBehaviourPunCallbacks
{
    GameObject PC;

    private void Awake()
    {
    }

    private void Start()
    {
        if (this.photonView.IsMine)
        {
            AudioManager.Instance.PlayBgm(true);
            CreatePlayer();
        }
    }

    void CreatePlayer()
    {
        StartCoroutine(RespawnCoroutine(2));
    }


    public override void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    public override void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }


    IEnumerator RespawnCoroutine(float spawnTime)
    {
        yield return new WaitForSeconds(spawnTime);
        Transform t = SpawnerManager.Instance.GetSpawnPoint();
        PC = PhotonNetwork.Instantiate(Path.Combine("Prefabs", "PlayerController"), t.position, t.rotation, 0, new object[] { this.photonView.ViewID});
        PC.GetComponent<PlayerController>().playerManager = this;
    }

    public void Die()
    {
        PhotonNetwork.Destroy(PC);
        CreatePlayer();
    }
}
