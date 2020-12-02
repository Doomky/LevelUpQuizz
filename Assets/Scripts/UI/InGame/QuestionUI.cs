using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Text = TMPro.TMP_Text;

public class QuestionUI : MonoBehaviour
{

    [Header("UI Elements")]
    [Space]
    [SerializeField] protected Text _questionTxt;

    protected void Update()
    {
        if (GameManager.Question != null)
            _questionTxt.text = GameManager.Question.Text;
    }
}
