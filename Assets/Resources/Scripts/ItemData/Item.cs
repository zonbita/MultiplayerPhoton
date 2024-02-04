using Photon.Pun;
using UnityEngine;


public abstract class Item : MonoBehaviourPunCallbacks
{
    public string Name;
    public string Description;
    public GameObject GunModel;

    public abstract void Use();
}
