using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Text = TMPro.TMP_Text;

public class PlayerUI : MonoBehaviour
{
    [Header("Data")]
    [Space]
    [SerializeField] protected Player _player;

    [Header("UI Element")]
    [Space]
    [SerializeField] protected Text _playerNameTxt;

    public void SetPlayer(Player player)
    {
        _player = player;
        _playerNameTxt.text = _player.NickName;
    }
}
