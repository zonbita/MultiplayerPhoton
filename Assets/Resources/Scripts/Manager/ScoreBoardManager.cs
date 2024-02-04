using Photon.Pun;
using Photon.Pun.UtilityScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBoardManager : MonoBehaviourPunCallbacks
{
    public GameObject playerScorePrefab;
    public Transform panel;

    Dictionary<int, GameObject> playerScore = new Dictionary<int, GameObject>();


    void Start()
    {
        foreach (var player in PhotonNetwork.PlayerList)
        {
            player.SetScore(0);

            var playerScoreObject = Instantiate(playerScorePrefab, panel);
            var playerScoreObjectText = playerScoreObject.GetComponent<Text>();

            playerScoreObjectText.text = string.Format("{0} Score: {1}", player.NickName, player.GetScore());

            playerScore[player.ActorNumber] = playerScoreObject;
        }
    }

    public override void OnPlayerPropertiesUpdate(Photon.Realtime.Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        var playerScoreObejct = playerScore[targetPlayer.ActorNumber];
        var playerScoreObjectText = playerScoreObejct.GetComponent<Text>();
        playerScoreObjectText.text = string.Format("{0} Score: {1}", targetPlayer.NickName, targetPlayer.GetScore());
    }
}
