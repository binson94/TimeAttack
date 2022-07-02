using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateBtn : MonoBehaviour
{

    //[SerializeField]
    //List<GameObject> buttons = new List<GameObject>();

    int[] levels = new int[]{ 1, 1, 1, 1, 1, 1 };
    public Text[] nowdata;
    public Text[] nextdata;
    public Text[] nowlevel;
    public Text[] nextlevel;
    public Text[] cost;

    public Button[] btns;

    [SerializeField] int Holdingmoney = 1000;


    private void Start()
    {

        for (int idx = 0;idx<6;idx++)
        {
            nowdata[idx].text = $"{GameManager.instance.GetStat(idx, levels[idx])}"; // 1 1
            nextdata[idx].text = $"{GameManager.instance.GetStat(idx, levels[idx] + 1)}";
            nowlevel[idx].text = $"LV.{levels[idx]}";
            nextlevel[idx].text = $"LV.{levels[idx] + 1}";
            cost[idx].text = $"{GameManager.instance.GetPrice(levels[idx])}";

            btns[idx].interactable = levels[idx] < 5;
        }
    }

    public void UpgradeBtn(int idx)
    {
        if (GameManager.instance.GetPrice(levels[idx]) < Holdingmoney)
            {
            SoundManager.instance.PlaySFX(SFXList.Upgrade_Success);

            Holdingmoney -= GameManager.instance.GetPrice(levels[idx]);
            levels[idx]++;
            nowdata[idx].text = $"{GameManager.instance.GetStat(idx, levels[idx])}"; // 1 1
            nextdata[idx].text = $"{GameManager.instance.GetStat(idx, levels[idx] + 1)}";
            nowlevel[idx].text = $"LV.{levels[idx]}";
            nextlevel[idx].text = $"LV.{levels[idx] + 1}";
            cost[idx].text = $"{GameManager.instance.GetPrice(levels[idx])}";

            btns[idx].interactable = levels[idx] < 5;
        }
        else
            SoundManager.instance.PlaySFX(SFXList.Upgrade_Fail);
    }

}
