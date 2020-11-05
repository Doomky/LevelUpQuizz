using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomFooterUI : MonoBehaviour
{
    [Header("UIElements")]
    [Space]
    [SerializeField] protected Button _startGameBtn;

    protected void OnEnable()
    {
        _startGameBtn.gameObject.SetActive(PhotonNetwork.IsMasterClient);
    }

    public void OnClickStartGameBtn()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.CurrentRoom.IsVisible = false;
            PhotonNetwork.CurrentRoom.IsOpen = false;
        }
    }
}
