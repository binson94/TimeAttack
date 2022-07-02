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
    }    
    
    public void UpgradeOnclick()
    {
        // ���׷��̵� â ����
        upgrade.SetActive(true);
        isReturn(); // ���� ��ư ���ֱ�
    }

    public void OptionOnClick()
    {
        // ���� â ����
        option.SetActive(true);
        isReturn();
    }

    public void ExitOnclick()
    {
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
        option.SetActive(false);
        upgrade.SetActive(false);
        returnbtn.SetActive(false);

    }
}
