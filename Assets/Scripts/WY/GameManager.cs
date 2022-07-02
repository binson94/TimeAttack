using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

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

    public int[] lvls;
    StatData statData;
    void LoadData()
    {
        lvls = new int[6];
        for(int i = 0;i < 6;i++)
            lvls[i] = PlayerPrefs.GetInt($"Stat{i}", 1);

        statData = new StatData();
    }

    public int GetStat(int statIdx, int statLvl) => statData.stats[statIdx, Mathf.Min(4, statLvl - 1)];
    public int GetPrice(int statLvl) => statData.requireGems[Mathf.Min(4, statLvl)];
}

public class StatData
{
    ///<summary> 0 atk, 1 fireRate, 2 health, 3 reflect, 4 speed, 5 shield </summary> 
    public int[,] stats = new int[6, 5];
    public int[] requireGems = new int[5];

    public StatData()
    {
        JsonData json = JsonMapper.ToObject(Resources.Load<TextAsset>("Jsons/Stat").text);

        for(int i = 0;i < 5;i++)
        {
            requireGems[i] = (int)json[i]["gem"];

            stats[0, i] = (int)json[i]["attack"];
            stats[1, i] = (int)json[i]["fireRate"];
            stats[2, i] = (int)json[i]["health"];
            stats[3, i] = (int)json[i]["reflect"];
            stats[4, i] = (int)json[i]["speed"];
            stats[5, i] = (int)json[i]["shield"];
        }
    }
}
