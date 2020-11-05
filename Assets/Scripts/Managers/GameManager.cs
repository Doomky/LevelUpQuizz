using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviourPunCallbacks
{
    public const float TIME_TO_ANSWER = 1;

    private static GameManager _singleton; 

    [Header("Data")]
    [Space]
    [SerializeField] protected float _timeLeft = TIME_TO_ANSWER;
    [SerializeField] protected int _questionNumber = 1;
    [SerializeField] protected Question _question;
    [SerializeField] protected Question.ResponseType? _selectedResponse;
    [SerializeField] protected int _gainedScore = 0;
    [SerializeField] protected int _score;
    [SerializeField] protected bool _isOver;

    public static float TimeLeft => _singleton._timeLeft;
    public static int QuestionNumber => _singleton._questionNumber;
    public static Question Question => _singleton._question;
    public static int Score => _singleton._score;
    public static bool IsOver => _singleton._isOver;

    public static void SelectAnswer(Question.ResponseType selectedReponse)
    {
        _singleton._selectedResponse = selectedReponse;
        _singleton._gainedScore = selectedReponse == _singleton._question.CorrectAnswer ? Mathf.RoundToInt(1000 * (_singleton._timeLeft/ TIME_TO_ANSWER)) : 0;
        AnswerUI.AnswerAUI.DisableBtn();
        AnswerUI.AnswerBUI.DisableBtn();
        AnswerUI.AnswerCUI.DisableBtn();
        AnswerUI.AnswerDUI.DisableBtn();
    }

    public override void OnEnable()
    {
        base.OnEnable();
        _singleton = this;
        _isOver = false;
        _timeLeft = TIME_TO_ANSWER;
        _questionNumber = 1;
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
            StartCoroutine(QuestionCoroutine());
    }

    private IEnumerator QuestionCoroutine()
    {
        yield return new WaitForSeconds(1);
        do
        {
            photonView.RPC("LoadQuestion", RpcTarget.All);

            while (_singleton._timeLeft > 0)
            {
                yield return new WaitForEndOfFrame();
                photonView.RPC("UpdateTimer", RpcTarget.All, _timeLeft - Time.deltaTime);
            }
            Debug.Log($"Question {_questionNumber} is over");
            photonView.RPC("DisplayAnwser", RpcTarget.All);

            Debug.Log($"Moving to {_questionNumber + 1} in 2 seconds");
            yield return new WaitForSeconds(2f);
            photonView.RPC("MovingToNextQuestion", RpcTarget.All);

        } while (_questionNumber <= 10);

        photonView.RPC("QuizzIsOver", RpcTarget.All);

        yield return 0;
    }

    [PunRPC]
    protected void QuizzIsOver()
    {
        Debug.Log("Quizz Is Over");
        _isOver = true;
    }

    [PunRPC]
    protected void UpdateTimer(float timeLeft)
    {
        _timeLeft = timeLeft;
        Debug.Log($"Question {_questionNumber}: {_timeLeft} seconds Left");
    }

    [PunRPC]
    protected void MovingToNextQuestion()
    {
        _questionNumber++;
    }

    [PunRPC]
    protected void LoadQuestion()
    {
        AnswerUI.AnswerAUI.ResetInteractionAndBackgroundColor();
        AnswerUI.AnswerBUI.ResetInteractionAndBackgroundColor();
        AnswerUI.AnswerCUI.ResetInteractionAndBackgroundColor();
        AnswerUI.AnswerDUI.ResetInteractionAndBackgroundColor();
        _selectedResponse = null;
        _timeLeft = TIME_TO_ANSWER;
        _question = new Question($"C'est la question {_questionNumber}",
           new Answer[4]
           {
               new Answer("réponse A"),
               new Answer("réponse B"),
               new Answer("réponse C"),
               new Answer("réponse D"),
           },
           Question.ResponseType.A);
    }

    [PunRPC]
    protected void DisplayAnwser()
    {
        _singleton._score += _singleton._gainedScore;
        _singleton._gainedScore = 0;

        // disable all buttons
        AnswerUI.AnswerAUI.DisableBtn();
        AnswerUI.AnswerBUI.DisableBtn();
        AnswerUI.AnswerCUI.DisableBtn();
        AnswerUI.AnswerDUI.DisableBtn();

        // show color
        AnswerUI.AnswerAUI.ShowAnswerBackgroundColor();
        AnswerUI.AnswerBUI.ShowAnswerBackgroundColor();
        AnswerUI.AnswerCUI.ShowAnswerBackgroundColor();
        AnswerUI.AnswerDUI.ShowAnswerBackgroundColor();
    }
}
