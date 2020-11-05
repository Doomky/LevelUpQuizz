using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Question
{
    public enum ResponseType
    {
        A = 0,
        B = 1,
        C = 2,
        D = 3
    }

    [Header("Data")]
    [Space]
    [SerializeField] protected string _text;
    [SerializeField] protected Answer[] _answers;
    [SerializeField] protected ResponseType _corectAnswer;

    public string Text => _text;
    public Answer AnswerA => _answers[(int)ResponseType.A];
    public Answer AnswerB => _answers[(int)ResponseType.B];
    public Answer AnswerC => _answers[(int)ResponseType.C];
    public Answer AnswerD => _answers[(int)ResponseType.D];
    public Answer[] Answers => _answers;
    public ResponseType CorrectAnswer => _corectAnswer;

    public Question(string text, Answer[] answers, ResponseType corectAnswer)
    {
        _text = text;
        _answers = answers;
        _corectAnswer = corectAnswer;
    }
}
