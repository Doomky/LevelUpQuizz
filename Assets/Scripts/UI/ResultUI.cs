using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

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

    private IEnumerator PostScore()
    {
        PostScoreDTORequest postScoreDTORequest = new PostScoreDTORequest(
            PhotonNetwork.LocalPlayer.NickName, 
            GameManager.Score
        );

        string bodyString = JsonUtility.ToJson(postScoreDTORequest);
        UnityWebRequest request = new UnityWebRequest();
        request.url = BackendURLs.BACKEND_BASE_URL + BackendURLs.BACKEND_POST_SCORE_ROUTE;
        request.method = UnityWebRequest.kHttpVerbPOST;
        request.downloadHandler = new DownloadHandlerBuffer();
        request.uploadHandler = new UploadHandlerRaw(string.IsNullOrEmpty(bodyString) ? null : Encoding.UTF8.GetBytes(bodyString));
        request.SetRequestHeader("Accept", "application/json");
        request.SetRequestHeader("Content-Type", "application/json");
        request.timeout = 60;
        yield return request.SendWebRequest();

        if (false && (request.isNetworkError || request.isHttpError))
        {
            Debug.LogError("SendRequestError:" + request.error);
        }
        else
        {
            Debug.Log(request.responseCode + " : " + request.downloadHandler.text);
        }
    }
    public IEnumerator DelayedOnEnable()
    {
        yield return new WaitForSeconds(0.2f);
        yield return PostScore();
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
