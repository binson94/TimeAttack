using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneBtns : MonoBehaviour
{
    public GameObject option;
    public GameObject upgrade;
    public GameObject returnbtn;


    public void StartOnclick()
    {
        SceneManager.LoadScene(1);
        SoundManager.instance.PlaySFX(SFXList.Button);
    }    
    
    public void UpgradeOnclick()
    {
        // 업그레이드 창 띄우기
        SoundManager.instance.PlaySFX(SFXList.Button);
        upgrade.SetActive(true);
        isReturn(); // 리턴 버튼 켜주기
    }

    public void OptionOnClick()
    {
        // 설정 창 띄우기
        SoundManager.instance.PlaySFX(SFXList.Button);
        option.SetActive(true);
        isReturn();
    }

    public void ExitOnclick()
    {
        SoundManager.instance.PlaySFX(SFXList.Button);
        // 게임 종료
    }

    public void isReturn()
    {
        if (option.activeSelf == true || upgrade.activeSelf == true)
        {
            returnbtn.SetActive(true);
        }
        else
            returnbtn.SetActive(false);
    }

    public void ReturnBtnOnClick()
    {
        SoundManager.instance.PlaySFX(SFXList.Button);
        option.SetActive(false);
        upgrade.SetActive(false);
        returnbtn.SetActive(false);

    }
}
