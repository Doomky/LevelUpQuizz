using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ScriptableObjectSingleton<T> : ScriptableObject  where T : ScriptableObject
{
    private static T _singleton;

    public static T Singleton
    {
        get
        {
            if (_singleton == null)
            {
                T[] ts = Resources.FindObjectsOfTypeAll<T>();
                if (ts.Length == 0)
                {
                    Debug.LogError("ScriptableObjectSingleton: No Instance for type: " + typeof(T).Name);
                    return null;
                }
                else if (ts.Length > 1)
                {
                    Debug.LogError("ScriptableObjectSingleton: More than 1 Instance for type: " + typeof(T).Name);
                    return null;
                }

                _singleton = ts[0];
            }
            return _singleton;
        }
    }
}
