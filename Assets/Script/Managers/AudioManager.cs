using UnityEngine;
using System.Collections.Generic;
using System;


[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public Sound[] backsounds, sfxSounds;
    public AudioSource backsoundSource, sfxSource;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        PlayExplorationBacksound();
    }

    public void PlayBacksound(string name)
    {
        Sound sound = Array.Find(backsounds, x => x.name == name);
        if (sound == null)
        {
            Debug.Log("BackSound not found");
        }
        else
        {
            backsoundSource.clip = sound.clip;
            backsoundSource.Play();
            Debug.Log("tes not found");
        }
    }

    public void PlaySFX(string name)
    {
        Sound sound = Array.Find(sfxSounds, x => x.name == name);
        if (sound == null)
        {
            Debug.Log("SFX not found");
        }
        else
        {
            sfxSource.PlayOneShot(sound.clip);
        }
    }


    // Play BackSound
    public void PlayExplorationBacksound() => PlayBacksound("Exploration");
    public void PlayMainThemeBacksound() => PlayBacksound("Main Theme");


    // Play SFX
    // public void PlayClikUI() => PlayBacksound("Click UI");
    public void PlayWalk() => PlaySFX("Walk");
    public void PlayDiving() => PlaySFX("Diving");
    public void PlayRun() => PlaySFX("Run");

    public void PlayClikUI()
    {
        PlaySFX("Click UI");
    }
}
