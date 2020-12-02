using Newtonsoft.Json.Linq;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class GameManager : MonoBehaviourPunCallbacks
{
    public const float TIME_TO_ANSWER = 8;

    private static GameManager _singleton;

    [Header("Data")]
    [Space]
    [SerializeField] protected GameObject _questionsLoadingScreen;
    [SerializeField] protected float _timeLeft = TIME_TO_ANSWER;
    [SerializeField] protected int _questionNumber = 1;
    protected Question[] _questions = new Question[10];
    protected Question _question;
    [SerializeField] protected Question.ResponseType? _selectedResponse;
    [SerializeField] protected int _gainedScore = 0;
    [SerializeField] protected int _score;
    [SerializeField] protected bool _isOver;
    [SerializeField] protected static bool _isSignedIn;

    public static GameManager Singleton => _singleton;
    public static float TimeLeft => _singleton._timeLeft;
    public static int QuestionNumber => _singleton._questionNumber;
    public static Question Question => _singleton._question;
    public static int Score => _singleton._score;
    public static bool IsOver => _singleton._isOver;
    public static bool IsSignedIn => _isSignedIn;

    public static void SignIn(string username, string password)
    {
        PhotonNetwork.LocalPlayer.NickName = username;
        _isSignedIn = true;
    }

    public static void SelectAnswer(Question.ResponseType selectedReponse)
    {
        _singleton._selectedResponse = selectedReponse;
        _singleton._gainedScore = selectedReponse == _singleton._question.CorrectAnswer ? Mathf.RoundToInt(1000 * (_singleton._timeLeft / TIME_TO_ANSWER)) : 0;
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
        _questionNumber = 0;
        _questionsLoadingScreen.SetActive(true);
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
            StartCoroutine(QuestionCoroutine());
    }

    private IEnumerator QuestionCoroutine()
    {
        yield return new WaitForSeconds(1f);
        GetRandomQuestionsFromBackend();
        
        while (!_questionsHaveBeenFetched)
            yield return new WaitForEndOfFrame();

        while (!PhotonNetwork.IsConnected)
            yield return new WaitForEndOfFrame();

        photonView.RPC("HideQuestionLoadingScreen", RpcTarget.All);
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

        } while (_questionNumber < 10);

        photonView.RPC("QuizzIsOver", RpcTarget.All);

        yield return 0;
    }

    [PunRPC]
    protected void QuizzIsOver()
    {
        _isOver = true;
    }

    [PunRPC]
    protected void UpdateTimer(float timeLeft)
    {
        _timeLeft = timeLeft;
    }

    [PunRPC]
    protected void MovingToNextQuestion()
    {
        _questionNumber++;
    }

    private bool _questionsHaveBeenFetched;

    private IEnumerator SendRequest()
    {
        _questionsHaveBeenFetched = false;

        UnityWebRequest www = UnityWebRequest.Get(BackendURLs.BACKEND_BASE_URL + BackendURLs.BACKEND_GET_QUESTIONS_ROUTE);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
            Debug.LogError("SendRequestError:" + www.error);
         
        string questionsAsJsonString = www.downloadHandler.text;

        JObject questionsAsJObject = JObject.Parse(questionsAsJsonString);

        JArray questionsAsJArray = (JArray)(questionsAsJObject["questions"]);

        for (int i = 0; i < questionsAsJArray.Count; i++)
        {
            Debug.Log(questionsAsJArray[i].ToString());
            photonView.RPC("SetQuestion", RpcTarget.All, i, questionsAsJArray[i].ToString());
        }

        _questionsHaveBeenFetched = true;
    }

    [PunRPC]
    private void SetQuestion(int questionNumber, string questionAsString)
    {
        JObject question = JObject.Parse(questionAsString);

        Answer[] answers = new Answer[] {
                new Answer((string)(question["ResponseA"])),
                new Answer((string)(question["ResponseB"])),
                new Answer((string)(question["ResponseC"])),
                new Answer((string)(question["ResponseD"])),
            };

        Question.ResponseType correctAnswer = (Question.ResponseType)Enum.Parse(typeof(Question.ResponseType), (string)(question["CorrectAnswer"]), true);

        _questions[questionNumber] = new Question(
            (string)(question["Text"]),
            answers,
            correctAnswer
        );
    }

    [PunRPC]
    protected void GetRandomQuestionsFromBackend()
    {
        StartCoroutine(SendRequest());
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
        _question = _questions[_questionNumber];
    }



    [PunRPC]
    protected void HideQuestionLoadingScreen()
    {
        _questionsLoadingScreen.SetActive(false);
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
