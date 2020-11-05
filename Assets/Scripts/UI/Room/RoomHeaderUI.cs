using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Text = TMPro.TMP_Text;

public class RoomHeaderUI : MonoBehaviour
{
    [Header("UI Elements")]
    [Space]
    [SerializeField] protected Text _roomNameTxt;

    private void OnEnable()
    {
        _roomNameTxt.text = PhotonNetwork.CurrentRoom.Name;
    }

    public void OnClickLeaveButton()
    {
        PhotonNetwork.LeaveRoom();
    }
}
