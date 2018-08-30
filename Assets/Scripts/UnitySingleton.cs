using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitySingleton<T> : MonoBehaviour where T : Component {

    private static T s_instance;
    public static T Instance {
        get {
            if (s_instance == null) {
                CreateInstance();
            }
            return s_instance;
        }
    }
    public static void CreateInstance() {
        s_instance = FindObjectOfType<T>() as T;
        if (s_instance == null) {
            GameObject o = new GameObject {
                name = typeof(T).ToString(),
                hideFlags = HideFlags.HideAndDontSave

            };
            s_instance = o.AddComponent<T>();
        }
    }

}


