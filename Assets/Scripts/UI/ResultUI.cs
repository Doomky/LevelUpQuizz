using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultUI : MonoBehaviourPunCallbacks
{
    [Header("Data")]
    [Space]
    [SerializeField] protected Transform _playerResultUIHolder;
    [SerializeField] protected GameObject _playerResultUIPrefab;
    [SerializeField] protected HashSet<PlayerResultUI> _playerResultUIs = new HashSet<PlayerResultUI>();


    public void OnEnable()
    {
        foreach (PlayerResultUI playerResultUI in _playerResultUIs)
            GameObject.Destroy(playerResultUI.gameObject);

        _playerResultUIs.Clear();

        StartCoroutine(DelayedOnEnable());
    }

    public IEnumerator DelayedOnEnable()
    {
        yield return new WaitForSeconds(0.8f);
        photonView.RPC("OnPlayerResultReceived", RpcTarget.All, PhotonNetwork.LocalPlayer.NickName, GameManager.Score);
    }

    public void OnPlayerResultAdded(string nickname, int score)
    {
        GameObject playerResultUIGo = GameObject.Instantiate(_playerResultUIPrefab, _playerResultUIHolder);
        PlayerResultUI playerResultUI = playerResultUIGo.GetComponent<PlayerResultUI>();
        playerResultUI.SetPlayerResult(nickname, score);
        _playerResultUIs.Add(playerResultUI);
    }

    [PunRPC]
    public void OnPlayerResultReceived(string nickname, int score)
    {
        OnPlayerResultAdded(nickname, score);
    }

    [PunRPC]
    public void LeaveRoomOnClick()
    {
        PhotonNetwork.LeaveRoom(true);
    }
}
