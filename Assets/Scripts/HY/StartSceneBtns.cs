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
        // ���׷��̵� â ����
        SoundManager.instance.PlaySFX(SFXList.Button);
        upgrade.SetActive(true);
        isReturn(); // ���� ��ư ���ֱ�
    }

    public void OptionOnClick()
    {
        // ���� â ����
        SoundManager.instance.PlaySFX(SFXList.Button);
        option.SetActive(true);
        isReturn();
    }

    public void ExitOnclick()
    {
        SoundManager.instance.PlaySFX(SFXList.Button);
        // ���� ����
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
