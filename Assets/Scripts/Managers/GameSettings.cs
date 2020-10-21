using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Manager/GameSettings")]
public class GameSettings : ScriptableObject
{
    public const string DEFAULT_USERNAME_BASE = "user_";
    public readonly Guid DEFAULT_USERNAME_GUID = Guid.NewGuid();


    [SerializeField] private string _gameVersion = "0.0.1";
    [SerializeField] private string _nickname = null;


    public string GameVersion => _gameVersion;
    public string Nickname => String.IsNullOrEmpty(_nickname) ? DEFAULT_USERNAME_BASE + DEFAULT_USERNAME_GUID.ToString() : _nickname;
}