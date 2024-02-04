using Photon.Realtime;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class RoomItem : MonoBehaviour, IPointerClickHandler
{
    RoomInfo roomInfo;
    public void OnPointerClick(PointerEventData eventData)
    {
        NetworkManager.Instance.JoinRoomClick(roomInfo);
    }

    public void Setup(RoomInfo roomInfo)
    {
        this.roomInfo = roomInfo;
        Text[] L = GetComponentsInChildren<Text>();

        L[0].text = roomInfo.Name;
        L[1].text = "Map_1";
        L[2].text = roomInfo.PlayerCount.ToString() + "/" + roomInfo.MaxPlayers.ToString();
    }
}
