using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;

public class Launcher : MonoBehaviourPunCallbacks
{
    [SerializeField] Transform playerSpawnPos;
    // Start is called before the first frame update
    private void Awake()
    {
        PhotonNetwork.ConnectUsingSettings();   //connect to the server
    }
    public override void OnConnectedToMaster()  
    {
        Debug.Log("Joined the server.");
        PhotonNetwork.JoinLobby();
    }
    public override void OnJoinedLobby()
    {
        Debug.Log("Joined Lobby");
        PhotonNetwork.JoinOrCreateRoom("room1", new RoomOptions
        {
            MaxPlayers = 4,
            IsOpen=true,
            IsVisible=true

        },TypedLobby.Default);
    }
    public override void OnJoinedRoom()
    {
        Debug.Log("Joined the room.");
        GameObject player = PhotonNetwork.Instantiate("player", playerSpawnPos.position , Quaternion.identity,0,null);
    }
    public override void OnLeftLobby()
    {
        base.OnLeftLobby();
    }
    public override void OnLeftRoom()
    {

    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        base.OnJoinRandomFailed(returnCode, message);
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
