using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    static SoundManager _instance = null;

    public static SoundManager instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject container = new GameObject();
                container.name = "Sound Manager";
                _instance = container.AddComponent<SoundManager>();

                _instance.mixer = Resources.Load<AudioMixer>("Mixer");

                _instance.bgmSource = _instance.MakeSource(container.transform, "BGM", true);
                _instance.sfxSource = _instance.MakeSource(container.transform, "SFX", false);

                _instance.LoadSource();

                DontDestroyOnLoad(container);
            }

            return _instance;
        }
    }
    AudioSource MakeSource(Transform parent, string typeName, bool loop)
    {
        GameObject container = new GameObject();
        container.name = typeName;
        AudioSource source = container.AddComponent<AudioSource>();

        source.playOnAwake = false;
        source.loop = loop;
        source.outputAudioMixerGroup = mixer.FindMatchingGroups(typeName)[0];
        container.transform.SetParent(parent);

        return source;
    }
    
    AudioMixer mixer;
    AudioSource bgmSource;
    AudioSource sfxSource;

    const int BGM_COUNT = 2;
    AudioClip[] bgmClips;
    const int SFX_COUNT = 9;
    AudioClip[] sfxClips;
    void LoadSource()
    {
        bgmClips = new AudioClip[BGM_COUNT];
        for(int i = 0;i < BGM_COUNT;i++)    bgmClips[i] = Resources.Load<AudioClip>($"Sounds/BGM_{i}");
        sfxClips = new AudioClip[SFX_COUNT];
        for (int i = 0; i < SFX_COUNT; i++) sfxClips[i] = Resources.Load<AudioClip>($"Sounds/SFX_{i}");
    }

    public void SetBGM(float value)
    {
        mixer.SetFloat("BGM", Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat("BGM", value);
    }
    public void SetSFX(float value)
    {
        mixer.SetFloat("SFX", Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat("SFX", value);
    }

    public void PlayBGM(BGMList bgm)
    {
        if(bgmSource.clip == bgmClips[(int)bgm]) return;

        bgmSource.clip = bgmClips[(int)bgm];
        bgmSource.Play();
    }

    public void PlaySFX(SFXList sfx)
    {
        sfxSource.PlayOneShot(sfxClips[(int)sfx]);
    }
}

public enum BGMList
{
    MainMenu, Ingame
}

public enum SFXList
{
    Button, Upgrade_Success, Upgrade_Fail, Gun_Shot, Hult_Shoter, Shield_Reflect, GameOver, Dead_Weak, Dead_Strong
}
