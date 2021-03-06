using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class AudioMNG : MonoBehaviour
{

    [SerializeField] Slider bgmSlider;
    [SerializeField] Slider sfxSlider;
    [SerializeField] Slider joystickSizeSlider;

    private void Start()
    {
        bgmSlider.value = PlayerPrefs.GetFloat("BGM", 1);
        sfxSlider.value = PlayerPrefs.GetFloat("SFX", 1);
        joystickSizeSlider.value = PlayerPrefs.GetFloat("Joystick", 0.5f);

        SetBGM();
        SetSFX();

        //SoundManager.instance.PlayBGM(BGMList.MainMenu);
        //SoundManager.instance.PlaySFX(SFXList.Button);

    }

    public void SetBGM()
    {
        SoundManager.instance.SetBGM(bgmSlider.value);
    }
    public void SetSFX()
    {
        SoundManager.instance.SetSFX(sfxSlider.value);
    }

    public void SetJoystick()
    {
        PlayerPrefs.SetFloat("Joystick", joystickSizeSlider.value);
    }
}
