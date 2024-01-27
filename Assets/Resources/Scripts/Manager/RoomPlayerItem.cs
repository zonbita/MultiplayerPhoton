using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
public class RoomPlayerItem : MonoBehaviourPunCallbacks
{
    [SerializeField] Text Nametext;
    Player player;
   
    public void Setup(Player p)
    {
        player = p;
        Nametext.text = p.NickName;
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        Destroy(this.gameObject);
    }

    public void OnPlayerLeftRoom()
    {
        Destroy(this.gameObject);
    }
}
