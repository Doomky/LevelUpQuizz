using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PostScoreDTORequest
{
    public string Login;
    public int Score;
    public PostScoreDTORequest(string login, int score)
    {
        Login = login;
        Score = score;
    }
}
