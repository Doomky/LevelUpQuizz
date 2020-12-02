using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviourPunCallbacks
{
    public enum State
    {
        SignIn,
        InLobby,
        InRoom,
        InGame,
        ResultPage
    }

    [SerializeField] protected GameObject _signInCanvas;
    [SerializeField] protected GameObject _lobbyCanvas;
    [SerializeField] protected GameObject _roomCanvas;
    [SerializeField] protected GameObject _inGameCanvas;
    [SerializeField] protected GameObject _resultCanvas;
    [SerializeField] protected State _state;

    public void Update()
    {
        switch (_state)
        {
            case State.SignIn:
                if (GameManager.IsSignedIn)
                    _state = State.InLobby;
                break;
            case State.InLobby:
                if (PhotonNetwork.InRoom)
                    _state = State.InRoom;
                break;
            case State.InRoom:
                if (!PhotonNetwork.InRoom)
                    _state = State.InLobby;
                if (!PhotonNetwork.CurrentRoom.IsOpen)
                    _state = State.InGame;
                break;
            case State.InGame:
                if (GameManager.IsOver)
                    _state = State.ResultPage;
                break;
            case State.ResultPage:
                if (!PhotonNetwork.InRoom)
                    _state = State.InLobby;
                break;
            default:
                break;
        }
        _signInCanvas.SetActive(_state == State.SignIn);
        _lobbyCanvas.SetActive(_state == State.InLobby);
        _roomCanvas.SetActive(_state == State.InRoom);
        _inGameCanvas.SetActive(_state == State.InGame);
        _resultCanvas.SetActive(_state == State.ResultPage);
    }
}
