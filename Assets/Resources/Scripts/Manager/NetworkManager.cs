using System;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using System.Collections.Generic;
using WebSocketSharp;
using UnityEngine.SceneManagement;
using System.Collections;

public class NetworkManager : Singleton<NetworkManager>
{
    [Header("-----------Game Object----------")]
    [SerializeField] GameObject LoginPanel;
    [SerializeField] GameObject CreateRoomPanel;
    [SerializeField] GameObject ServerListPanel;
    [SerializeField] GameObject ServerListPanelGrid;
    [SerializeField] GameObject ServerListPanelChild;
    [SerializeField] GameObject LoadingPanel;
    [SerializeField] GameObject RoomPlayersPanel;
    [SerializeField] GameObject RoomPlayersPanelGrid;
    [SerializeField] GameObject RoomPlayersPanelChild;
    [Header("InputField"), SerializeField] InputField UserName;
    [Header("Text"), SerializeField] Text StateText, connectionText, userText;
    [SerializeField] GameObject StartGameButton;

    [SerializeField]
    float loadScenePercent;
    Image LoadingImage;
    string randomName = $"NewPlayer{Guid.NewGuid().ToString()}";

    private void Awake()
    {
        LoadingImage = LoadingPanel.GetComponentInChildren<Image>();
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    private void Update()
    {
        StateText.text = PhotonNetwork.NetworkClientState.ToString();

    }
    public virtual void Login()
    {
        PhotonNetwork.LocalPlayer.NickName = (UserName.text.IsNullOrEmpty() ? randomName : UserName.text);
        PhotonNetwork.AuthValues = new AuthenticationValues(PhotonNetwork.LocalPlayer.NickName);
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.NickName = PhotonNetwork.LocalPlayer.NickName;
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        LoginPanel.SetActive(false);
        CreateRoomPanel.SetActive(true);
        userText.text = PhotonNetwork.LocalPlayer.NickName;

        if (!PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby();
        }

    }

    IEnumerator DelayedRoomCreation()
    {
        yield return new WaitUntil(() => PhotonNetwork.InLobby);

        CreateRoomClick();
    }

    public virtual void CreateRoomClick()
    {
   
        RoomOptions roomOptions = new RoomOptions()
        {
            IsOpen = true,
            IsVisible = true,
            MaxPlayers = 8
        };
        PhotonNetwork.CreateRoom("ROOM " + PhotonNetwork.LocalPlayer.NickName, roomOptions);
        connectionText.text = "Room : " + PhotonNetwork.LocalPlayer.NickName;

   
    }

    public override void OnCreatedRoom()
    {
    }

    public virtual void JoinRoomClick(RoomInfo roomInfo)
    {
        connectionText.text = "Joining room...";

        if (PhotonNetwork.IsConnectedAndReady)
        {
            RoomOptions roomOptions = new RoomOptions()
            {
                IsVisible = true,
                MaxPlayers = 8
            };
            PhotonNetwork.JoinOrCreateRoom(roomInfo.Name, roomOptions, TypedLobby.Default);
        }
        else
        {
            connectionText.text = "Connection is not ready, try restart it.";
        }

        StartGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }
    public override void OnJoinedRoom()
    {

        CreateRoomPanel.SetActive(false);
        RoomPlayersPanel.SetActive(true);

        Player[] players = PhotonNetwork.PlayerList;

        // Remove list
        int count = RoomPlayersPanelGrid.transform.childCount;
        for (int i = 0; i < count; i++)
        {
            Destroy(RoomPlayersPanelGrid.transform.GetChild(i).gameObject);
        }

        foreach (Player newPlayer in players)
        {
            Instantiate(RoomPlayersPanelChild, RoomPlayersPanelGrid.transform).GetComponent<RoomPlayerItem>().Setup(newPlayer);
        }

        StartGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    public void StartGameClick()
    {
  
        PhotonNetwork.LoadLevel("Map_1");
        //StartCoroutine(LoadAsyncScene("Map_1"));
    }

    public override void OnRoomListUpdate(List<RoomInfo> rooms)
    {

        // Remove list
        int count = ServerListPanelGrid.transform.childCount;
        for (int i = 0; i < count; i++)
        {
            Destroy(ServerListPanelGrid.transform.GetChild(i).gameObject);
        } 
            
        // Create server list
        foreach (RoomInfo r in rooms)
        {
            if (r.RemovedFromList) continue;
            Instantiate(ServerListPanelChild, ServerListPanelGrid.transform).GetComponent<RoomItem>().Setup(r);
        }
    }

    public void RefreshClick()
    {
        PhotonNetwork.GetCustomRoomList(TypedLobby.Default, "1=1");
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        // Add Panel child
        Instantiate(RoomPlayersPanelChild, RoomPlayersPanelGrid.transform).GetComponent<RoomPlayerItem>().Setup(newPlayer);
    }

    public override void OnPlayerLeftRoom(Player other)
    {
        if (PhotonNetwork.IsMasterClient)
        {
           // NoticeBoard.Instance.AddMessage("Player " + other.NickName + " Left Game.");
        }
    }

    public override void OnJoinedLobby()
    {
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        RoomPlayersPanel.SetActive(false);
        ServerListPanel.SetActive(false);
        PhotonNetwork.Disconnect();
        Login();
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log(returnCode + "," + message);
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        StartGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    public void ShowServerList(bool b)
    {
        ServerListPanel.SetActive(b);
    }

    IEnumerator LoadAsyncScene(string SceneName)
    {
        CreateRoomPanel.SetActive(false);
        ServerListPanel.SetActive(false);
        LoadingPanel.SetActive(true);
        float LoadTime = 0;
        
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(SceneName);
        while (!asyncOperation.isDone)
        {
            
            loadScenePercent = Mathf.Clamp01(asyncOperation.progress / 0.9f);
            LoadingImage.fillAmount = loadScenePercent;
            if (LoadTime < 2)
            {
                LoadTime += Time.deltaTime;
            }
            else {
                
                yield return null;
            } 
        }
    }
}