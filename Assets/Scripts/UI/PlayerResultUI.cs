using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Text = TMPro.TMP_Text;

public class PlayerResultUI : MonoBehaviour
{
    [Header("UI Elements")]
    [Space]
    [SerializeField] protected Text _nicknameTxt;
    [SerializeField] protected Text _scoreTxt;

    public void SetPlayerResult(string nickname, int score)
    {
        _nicknameTxt.text = nickname;
        _scoreTxt.text = score.ToString();
    }
}
