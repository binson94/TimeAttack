using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateBtn : MonoBehaviour
{
    ///<summary> 스텟 수치 표기 텍스트들 </summary>
    public Text[] nowStatTxts;
    ///<summary> 화살표 이미지 </summary>
    public GameObject[] arrowImages;
    public Text[] nextStatTxts;
    ///<summary> 현재 레벨 표기 텍스트들 </summary>
    public Text[] nowLvlTxts;
    ///<summary> 다음 레벨 표기 텍스트들 </summary>
    public Text[] nextLvlTxts;
    ///<summary> 비용 표기 텍스트들 </summary>
    public Text[] costTxts;

    ///<summary> 업그레이드 진행 버튼, 최대 레벨 도달 시 비활성화 </summary>
    public GameObject[] upgradeBtns;

    public Text holdingMoneyTxt;

    ///<summary> 현재 레벨 수치 불러오기 </summary>
    private void Start()
    {
        holdingMoneyTxt.text = GameManager.instance.holdingMoney.ToString();

        for (int idx = 0; idx < 6; idx++)
        {
            nowLvlTxts[idx].text = $"LV.{GameManager.instance.lvls[idx]}";

            nowStatTxts[idx].text = $"{GameManager.instance.GetStat(idx, GameManager.instance.lvls[idx])}";

            if(idx == 1)
            {
                nowStatTxts[idx].text += "/s";
                nowStatTxts[idx].rectTransform.sizeDelta = new Vector2(22 * (nowStatTxts[idx].text.Length - 1), 40);
            }
            else
                nowStatTxts[idx].rectTransform.sizeDelta = new Vector2(22 * nowStatTxts[idx].text.Length, 40);

            if (GameManager.instance.lvls[idx] < 5)
            {
                nextStatTxts[idx].text = $"{GameManager.instance.GetStat(idx, GameManager.instance.lvls[idx] + 1)}";
                if(idx == 1) nextStatTxts[idx].text += "/s";
                nextLvlTxts[idx].text = $"Lv.{GameManager.instance.lvls[idx] + 1}";
                costTxts[idx].text = $"{GameManager.instance.GetPrice(GameManager.instance.lvls[idx])}";
            }
            else
            {
                nextStatTxts[idx].text = string.Empty;
                nextLvlTxts[idx].text = string.Empty;
                costTxts[idx].text = string.Empty;

                arrowImages[idx].SetActive(false);
                upgradeBtns[idx].SetActive(false);
            }
        }
    }
    public void HoldingMoneyUpdate() => holdingMoneyTxt.text = GameManager.instance.holdingMoney.ToString();
    ///<summary> 업그레이드 진행 버튼 </summary>
    ///<param name="idx"> 0 atk, 1 fireRate, 2 health, 3 reflect, 4 speed, 5 shield </param>
    public void UpgradeBtn(int idx)
    {
        //현재 돈 수치와 업그레이드 비용 비교
        int price = GameManager.instance.GetPrice(GameManager.instance.lvls[idx]);

        if (price != 0 && price <= GameManager.instance.holdingMoney)
        {
            //성공 효과음 재생
            SoundManager.instance.PlaySFX(SFXList.Upgrade_Success);
            GameManager.instance.Upgrade(idx);
            holdingMoneyTxt.text = GameManager.instance.holdingMoney.ToString();

            nowLvlTxts[idx].text = $"Lv.{GameManager.instance.lvls[idx]}";

            nowStatTxts[idx].text = $"{GameManager.instance.GetStat(idx, GameManager.instance.lvls[idx])}";

            if(idx == 1)
            {
                nowStatTxts[idx].text += "/s";
                nowStatTxts[idx].rectTransform.sizeDelta = new Vector2(22 * (nowStatTxts[idx].text.Length - 1), 40);
            }
            else
                nowStatTxts[idx].rectTransform.sizeDelta = new Vector2(22 * nowStatTxts[idx].text.Length, 40);

            if (GameManager.instance.lvls[idx] < 5)
            {
                nextStatTxts[idx].text = $"{GameManager.instance.GetStat(idx, GameManager.instance.lvls[idx] + 1)}";
                if(idx == 1) nextStatTxts[idx].text += "/s";
                nextLvlTxts[idx].text = $"Lv.{GameManager.instance.lvls[idx] + 1}";
                costTxts[idx].text = $"{GameManager.instance.GetPrice(GameManager.instance.lvls[idx])}";
            }
            else
            {
                nextStatTxts[idx].text = string.Empty;
                nextLvlTxts[idx].text = string.Empty;
                costTxts[idx].text = string.Empty;

                arrowImages[idx].SetActive(false);
                upgradeBtns[idx].SetActive(false);
            }
        }
        else
            SoundManager.instance.PlaySFX(SFXList.Upgrade_Fail);
    }

}
