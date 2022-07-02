using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    static GameManager _instance = null;
    public static GameManager instance
    {
        get
        {
            if(_instance == null)
            {
                GameObject go = new GameObject();
                go.name = "Game Manager";
                _instance = go.AddComponent<GameManager>();
                _instance.LoadData();

                DontDestroyOnLoad(go);
            }

            return _instance;
        }
    }

    void LoadData()
    {
        
    }
}

