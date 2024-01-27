using Photon.Pun;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoticeBoard : MonoBehaviourPunCallbacks
{
    public static NoticeBoard Instance;
    private void Awake() => Instance = this;
    
    private Queue<string> messages;
    private const int Count = 10;

    [SerializeField] private InputField messagesLog;

    public void AddMessage(string message)
    {
        photonView.RPC("AddMessage_RPC", RpcTarget.All, message);
    }

    void AddMessage_RPC(string message)
    {
        messages.Enqueue(message);
        if (messages.Count > Count)
        {
            messages.Dequeue();
        }
        messagesLog.text = "";
        foreach (string m in messages)
        {
            messagesLog.text += m + "\n";
        }
    }
}
