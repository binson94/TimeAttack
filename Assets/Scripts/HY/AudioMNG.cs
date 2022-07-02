using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class AudioMNG : MonoBehaviour
{
    [SerializeField] AudioMixer mixer;

    [SerializeField] private AudioSource BGMPlayer;
    [SerializeField] private AudioSource SFXPlayer;

    [SerializeField] Slider bgmSlider;
    [SerializeField] Slider sfxSlider;

    //
    public AudioClip[] sfxAudioClips;


    public void SetBGM()
    {
        mixer.SetFloat("BGM",Mathf.Log10(bgmSlider.value) * 20 );
    }

}
