using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneBtns : MonoBehaviour
{
    public GameObject option;

    void Start()
    {
        
    }
    void Update()
    {
            
    }

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
        // 설정 창 띄우기
    }

    public void ExitOnclick()
    {
        // 게임 종료
    }
}
