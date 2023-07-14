using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T: Singleton<T>
{
    private static T instance;
    public static T Instance { get { return instance; } } // public getter calls instance


    // ProtectedV = only visible to class and those that inherit it
    protected virtual void Awake(){
        // assign instance to the monobehavior class this is attached to
        if (instance != null && this.gameObject != null){
            Destroy(this.gameObject);
        } else {
            instance = (T)this; // T is the generic type cast
        }

        if (!gameObject.transform.parent){
            DontDestroyOnLoad(gameObject);
        }
    }
}
