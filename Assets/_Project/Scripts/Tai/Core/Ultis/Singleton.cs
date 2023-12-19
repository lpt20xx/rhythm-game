using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Singleton<T> where T : new()
{
    private static T singleton = new T();

    public static T Instance
    {
        get
        {
            return singleton;
        }
    }
}
public class SingletonMono<T> : MonoBehaviour where T : UnityEngine.Component
{

    private void Reset()
    {
        gameObject.name = typeof(T).ToString();
    }

    private static T singleton;

    public static T Instance
    {
        get
        {
            if (SingletonMono<T>.singleton == null)
            {
                SingletonMono<T>.singleton = (T)FindObjectOfType(typeof(T));
                if (SingletonMono<T>.singleton == null)
                {
                    GameObject go = new GameObject();
                    go.name = typeof(T).ToString();
                    SingletonMono<T>.singleton = go.AddComponent<T>();

                }
            }
            return SingletonMono<T>.singleton;
        }
    }

}
