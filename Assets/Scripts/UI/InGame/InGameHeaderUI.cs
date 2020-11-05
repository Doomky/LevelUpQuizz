using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Text = TMPro.TMP_Text;

public class InGameHeaderUI : MonoBehaviour
{
    [Header("UI Elements")]
    [Space]
    [SerializeField] protected Text _scoreTxt;
    [SerializeField] protected Text _timeLeftTxt;
    [SerializeField] protected Slider _timeLeftSlider;
    [SerializeField] protected Image _timeLeftSliderBarImg;
    [SerializeField] protected Text _questionNumberTxt;

    [Header("UI Parameters")]
    [Space]
    [SerializeField] Color _highColor = Color.green;
    [SerializeField] Color _midColor = Color.yellow;
    [SerializeField] Color _lowColor = Color.red;

    protected Color GetCurrentColor()
    {
        float timeLeft = GameManager.TimeLeft;
        if (timeLeft < GameManager.TIME_TO_ANSWER * 0.33f)
            return _lowColor;
        else if (timeLeft < GameManager.TIME_TO_ANSWER * 0.5f)
            return _midColor;
        return _highColor;
    }

    protected void Update()
    {
        _timeLeftTxt.text = (Mathf.Ceil(GameManager.TimeLeft)).ToString();
        _timeLeftSlider.value = GameManager.TimeLeft / GameManager.TIME_TO_ANSWER;
        _timeLeftSliderBarImg.color = GetCurrentColor();
        _questionNumberTxt.text = $"Question {GameManager.QuestionNumber} :";
        _scoreTxt.text = $"Score: {GameManager.Score}";
    }
}
