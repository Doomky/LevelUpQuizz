using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Question;
using Text = TMPro.TMP_Text;

public class AnswerUI : MonoBehaviour
{
    private static Dictionary<ResponseType, AnswerUI> _responseTypeToUI = new Dictionary<ResponseType, AnswerUI>();

    public static AnswerUI AnswerAUI => _responseTypeToUI[ResponseType.A];
    public static AnswerUI AnswerBUI => _responseTypeToUI[ResponseType.B];
    public static AnswerUI AnswerCUI => _responseTypeToUI[ResponseType.C];
    public static AnswerUI AnswerDUI => _responseTypeToUI[ResponseType.D];

    [Header("Data")]
    [Space]
    [SerializeField] protected ResponseType _responseType;

    [Header("UI Elements")]
    [Space]
    [SerializeField] protected Button _btn;
    [SerializeField] protected Image _backgroundImg;
    [SerializeField] protected Text _answerTxt;

    [Header("UI Params")]
    [Space]
    [SerializeField] protected Color _selectedColor = Color.grey;
    [SerializeField] protected Color _correctColor = Color.green;
    [SerializeField] protected Color _wrongColor = Color.red;

    protected void Start()
    {
        _responseTypeToUI.Add(_responseType, this);
    }

    protected void Update()
    {
        if (GameManager.Question != null)
            _answerTxt.text = GameManager.Question.Answers[(int)_responseType].Text;
    }

    public void DisableBtn()
    {
        _btn.interactable = false;
    }

    public void AnswerBtnOnClick()
    {
        _backgroundImg.color = _selectedColor;
        GameManager.SelectAnswer(_responseType);
    }

    public void ShowAnswerBackgroundColor()
    {
        _backgroundImg.color = _responseType == GameManager.Question.CorrectAnswer ? _correctColor : _wrongColor;
    }

    public void ResetInteractionAndBackgroundColor()
    {
        _btn.interactable = true;
        _backgroundImg.color = Color.white;
    }
}
