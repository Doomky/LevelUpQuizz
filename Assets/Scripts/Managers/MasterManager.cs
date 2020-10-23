using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterManager : MonoBehaviour
{
    private static MasterManager _singleton;

    public static MasterManager Singleton
    {
        get
        {
            if (_singleton == null)
                _singleton = GameObject.FindObjectOfType<MasterManager>();
            return _singleton;
        }
    }

    [SerializeField] private GameSettings _gameSettings;

    public static GameSettings GameSettings => Singleton._gameSettings;
}
