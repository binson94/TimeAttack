using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneBtns : MonoBehaviour
{
    public GameObject option;
    public GameObject upgrade;

    private void Start() 
    {
        SoundManager.instance.PlayBGM(BGMList.MainMenu);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
            if(upgrade.activeInHierarchy || option.activeInHierarchy) ReturnBtnOnClick();
            else ExitOnclick();
    }


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
    }

    public void OptionOnClick()
    {
        // 설정 창 띄우기
        SoundManager.instance.PlaySFX(SFXList.Button);
        option.SetActive(true);
    }

    public void ExitOnclick()
    {
        SoundManager.instance.PlaySFX(SFXList.Button);
        Application.Quit();
    }

    public void ReturnBtnOnClick()
    {
        SoundManager.instance.PlaySFX(SFXList.Button);
        option.SetActive(false);
        upgrade.SetActive(false);
    }
}
