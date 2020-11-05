﻿using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerListUI : MonoBehaviourPunCallbacks
{
    [Header("UI Element")]
    [Space]
    [SerializeField] protected Transform _playersHolder;
    [SerializeField] protected GameObject _playerUIPrefab;
    [SerializeField] protected Dictionary<int, PlayerUI> _playerUIs = new Dictionary<int, PlayerUI>();

    public override void OnEnable()
    {
        base.OnEnable();
        //foreach (PlayerUI playerUI in _playerUIs.Values)
        //    GameObject.Destroy(playerUI.gameObject);
        foreach (Player player in PhotonNetwork.CurrentRoom.Players.Values)
            OnPlayerEnteredRoom(player);
    }

    public override void OnDisable()
    {
        base.OnDisable();
        foreach (PlayerUI playerUI in _playerUIs.Values)
            GameObject.Destroy(playerUI.gameObject);
        _playerUIs.Clear();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log($"{newPlayer.NickName} has entered room");

        if (!_playerUIs.ContainsKey(newPlayer.ActorNumber))
        {
            GameObject playerUIGo = GameObject.Instantiate(_playerUIPrefab, _playersHolder);
            PlayerUI playerUI = playerUIGo.GetComponent<PlayerUI>();
            playerUI.SetPlayer(newPlayer);
            _playerUIs.Add(newPlayer.ActorNumber, playerUI);
        }
        else
        {
            GameObject playerUIGo = GameObject.Instantiate(_playerUIPrefab, _playersHolder);
            GameObject.Destroy(_playerUIs[newPlayer.ActorNumber].gameObject);
            PlayerUI playerUI = playerUIGo.GetComponent<PlayerUI>();
            playerUI.SetPlayer(newPlayer);
            _playerUIs[newPlayer.ActorNumber] = playerUI;
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (_playerUIs.ContainsKey(otherPlayer.ActorNumber))
        {
            Debug.Log($"{otherPlayer.NickName} has left room");
            PlayerUI playerUI = _playerUIs[otherPlayer.ActorNumber];

            _playerUIs.Remove(otherPlayer.ActorNumber);

            GameObject.Destroy(playerUI.gameObject);
        }
    }
}
