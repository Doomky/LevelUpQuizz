using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Text = TMPro.TMP_Text;
using Photon.Pun;
using Photon.Realtime;
using System;

public class CreateRoomUI : MonoBehaviourPunCallbacks
{
    [Header("UI Element")]
    [Space]
    [SerializeField] protected Text _roomNameTxt;

    private string GetDefaultRoomName()
    {
        return $"{PhotonNetwork.LocalPlayer.NickName}'s Room";
    }

    public void OnClickCreateRoomButton()
    {
        if (!PhotonNetwork.IsConnected)
        {
            Debug.LogError("Cannot Create room: not connected");
            return;
        }

        RoomOptions roomOptions = new RoomOptions()
        {
            MaxPlayers = 10,
            EmptyRoomTtl = 0,
            PlayerTtl = 2,
            IsVisible = true,
            IsOpen = true,            
        };

        string[] users =
        {
             PhotonNetwork.LocalPlayer.UserId
        };

        string roomName = _roomNameTxt.text != null ? _roomNameTxt.text : GetDefaultRoomName() ;

        PhotonNetwork.CreateRoom(roomName, roomOptions, TypedLobby.Default, users);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Room Joined");
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("Created Room");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log($"Created Room Failed: ERR {returnCode} : {message}");
    }
}