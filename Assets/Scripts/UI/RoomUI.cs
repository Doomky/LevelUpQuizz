using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Text = TMPro.TMP_Text;

public class RoomUI : MonoBehaviour
{
    [Header("Data")]
    [Space]
    [SerializeField] protected RoomInfo _roomInfo;

    [Header("UI Element")]
    [Space]
    [SerializeField] protected Text _roomNameTxt;
    [SerializeField] protected Text _fillTxt;

    public void UpdateWithRoomInfo(RoomInfo roomInfo)
    {
        _roomInfo = roomInfo;
        _roomNameTxt.text = _roomInfo.Name;
        _fillTxt.text = $"{_roomInfo.PlayerCount} / {_roomInfo.MaxPlayers}";
    }

    public void OnClickJoinButton()
    {

    }

}
