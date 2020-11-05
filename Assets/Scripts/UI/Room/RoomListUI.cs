using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomListUI : MonoBehaviourPunCallbacks
{
    [Header("UI Element")]
    [Space]
    [SerializeField] protected Transform _roomsHolder;
    [SerializeField] protected GameObject _roomUIPrefab;
    [SerializeField] protected List<RoomUI> _roomUIs;

    public override void OnRoomListUpdate(List<RoomInfo> roomInfoList)
    {
        // deleting useless Room UI
        if (_roomUIs.Count > roomInfoList.Count)
        {
            List<RoomUI> roomUIToDestroy = _roomUIs.GetRange(roomInfoList.Count, _roomUIs.Count - roomInfoList.Count);
            foreach (var roomUI in roomUIToDestroy)
                GameObject.Destroy(roomUI);
            _roomUIs.RemoveRange(roomInfoList.Count, _roomUIs.Count - roomInfoList.Count);
        }

        // update room UI
        for (int i = 0; i < _roomUIs.Count; i++)
            _roomUIs[i].UpdateWithRoomInfo(roomInfoList[i]);

        // generate and update missing room UI
        for (int i = _roomUIs.Count; i < roomInfoList.Count; i++)
        {
            GameObject roomUIGo = GameObject.Instantiate(_roomUIPrefab, _roomsHolder);
            RoomUI roomUI = roomUIGo.GetComponent<RoomUI>();
            roomUI.UpdateWithRoomInfo(roomInfoList[i]);
            _roomUIs.Add(roomUI);
        }
    }

    void Update()
    {
        if (PhotonNetwork.InLobby)
            Debug.Log("In Lobby");

        if (PhotonNetwork.InRoom) 
            Debug.Log("In Room");
    }
}
