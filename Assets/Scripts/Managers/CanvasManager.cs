using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviourPunCallbacks
{
    public enum State
    {
        InLobby,
        InRoom
    }

    [SerializeField] protected GameObject _lobbyCanvas;
    [SerializeField] protected GameObject _roomCanvas;
    [SerializeField] protected State _state;

    public void Update()
    {
        switch (_state)
        {
            case State.InLobby:
                if (PhotonNetwork.InRoom)
                    _state = State.InRoom;
                break;
            case State.InRoom:
                if (!PhotonNetwork.InRoom)
                    _state = State.InLobby;
                break;
            default:
                break;
        }
        _lobbyCanvas.SetActive(_state == State.InLobby);
        _roomCanvas.SetActive(_state == State.InRoom);
    }
}
