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
            {
                MasterManager[] ts = Resources.FindObjectsOfTypeAll<MasterManager>();
                if (ts.Length == 0)
                {
                    Debug.LogError("ScriptableObjectSingleton: No Instance for type: " + typeof(MasterManager).Name);
                    return null;
                }
                else if (ts.Length > 1)
                {
                    Debug.LogError("ScriptableObjectSingleton: More than 1 Instance for type: " + typeof(MasterManager).Name);
                    return null;
                }

                _singleton = ts[0];
            }
            return _singleton;
        }
    }

    [SerializeField] private GameSettings _gameSettings;

    public static GameSettings GameSettings => Singleton._gameSettings;
}
