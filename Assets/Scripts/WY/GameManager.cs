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
    ///<summary> 레벨 정보 및 스텟 정보 불러오기 </summary>
    void LoadData()
    {
        lvls = new int[6];
        for(int i = 0;i < 6;i++)
            lvls[i] = PlayerPrefs.GetInt($"Stat{i}", 1);

        statData = new StatData();
    }
    ///<summary> 레벨 정보 저장하기 </summary>
    public void SaveData()
    {
        for(int i = 0;i < 6;i++)
            PlayerPrefs.SetInt($"Stat{i}", lvls[i]);
    }
    
    ///<summary> 현재 레벨의 스텟 수치 반환 </summary>
    ///<param name = "statIdx"> 0 atk, 1 fireRate, 2 health, 3 reflect, 4 speed, 5 shield </param>
    public int GetCurrStat(int statIdx) => statData.stats[statIdx, lvls[statIdx] - 1];
    
    ///<summary> 스텟 수치 반환 </summary>
    ///<param name = "statIdx"> 0 atk, 1 fireRate, 2 health, 3 reflect, 4 speed, 5 shield </param>
    public int GetStat(int statIdx, int statLvl) => statData.stats[statIdx, Mathf.Min(4, statLvl - 1)];
    public int GetPrice(int statLvl) => statData.requireGems[Mathf.Min(4, statLvl - 1)];
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
