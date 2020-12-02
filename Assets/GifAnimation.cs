using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GifAnimation : MonoBehaviour
{
    [SerializeField] protected Sprite[] sprites;
    [SerializeField] protected int framesPerSecond = 1;

    protected Image _img;

    protected void Start()
    {
        _img = GetComponent<Image>();
    }

    protected void Update()
    {
        int index = Mathf.FloorToInt((Time.time * framesPerSecond) % sprites.Length);
        _img.sprite = sprites[index];
    }
}
