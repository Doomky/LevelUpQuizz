using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Answer
{
    [Header("Data")]
    [Space]
    [SerializeField] protected string _text;

    public string Text => _text;

    public Answer(string text)
    {
        _text = text;
    }
}
