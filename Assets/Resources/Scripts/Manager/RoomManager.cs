using Photon.Pun;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine;


public class RoomManager : MonoBehaviourPunCallbacks
{
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    public override void OnEnable()
    {
        base.OnEnable();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.Instantiate(Path.Combine("Prefabs", "PlayerManagers"), Vector3.zero, Quaternion.identity, 0);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        if(scene.buildIndex == 1)
        {
            PhotonNetwork.Instantiate(Path.Combine("Prefabs", "PlayerManagers"), Vector3.zero, Quaternion.identity, 0);
            
        }
    }

}
