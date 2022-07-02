using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverPanel : MonoBehaviour
{
    public void Btn_Retry() => UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    public void Btn_Return() => UnityEngine.SceneManagement.SceneManager.LoadScene(0);
}
