using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using InputField = TMPro.TMP_InputField;

public class LoginUI : MonoBehaviour
{
    [Header("UI Elements")]
    [Space]
    [SerializeField] protected InputField _loginInput;
    [SerializeField] protected InputField _passwordInput;
    [SerializeField] protected Button _loginBtn;

    public void OnClickLoginBtn()
    {
        StartCoroutine(LoginRequest());
    }

    private bool _requestInProgress;

    [Serializable]
    public class LoginObject
    {
        [JsonProperty("login")]
        public string Login;
        [JsonProperty("emailAddress")]
        public string EmailAddress; 
        [JsonProperty("passwordHash")]
        public string PasswordHash;

        public LoginObject(string login, string emailAddress, string passwordHash)
        {
            Login = login;
            EmailAddress = emailAddress;
            PasswordHash = passwordHash;
        }
    }

    private IEnumerator LoginRequest()
    {
        if (!_requestInProgress)
        {
            _requestInProgress = true;
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] passwordHash = sha256.ComputeHash(Encoding.UTF8.GetBytes(_passwordInput.text));
                string passwordHashString = Encoding.UTF8.GetString(passwordHash, 0, passwordHash.Length);
                string bodyString = JsonUtility.ToJson(new LoginObject(_loginInput.text, "", passwordHashString));
                UnityWebRequest request = new UnityWebRequest();
                request.url = BackendURLs.BACKEND_BASE_URL + BackendURLs.BACKEND_POST_SIGN_IN_ROUTE;
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
                    _requestInProgress = false;
                }
                else
                {
                    Debug.Log(request.responseCode + " : " + request.downloadHandler.text);
                    _requestInProgress = false;
                    GameManager.SignIn(_loginInput.text, _passwordInput.text);
                }
            }
        }
    }
}
