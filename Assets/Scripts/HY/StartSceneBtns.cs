using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneBtns : MonoBehaviour
{
    public GameObject option;
    public GameObject exit;


    public void StartOnclick()
    {
        SceneManager.LoadScene(1);
    }    
    
    public void UpgradeOnclick()
    {
        SceneManager.LoadScene(2);
    }

    public void OptionOnClick()
    {
        option.SetActive(true);
        exit.SetActive(false);
    }

    public void ExitOnclick()
    {
        // 게임 종료
    }
    public void ReturntoMenu()
    {
        option.SetActive(false);
        exit.SetActive(true);
    }
}
