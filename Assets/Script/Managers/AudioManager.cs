using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance; 

    [Header("Background Music")]
    public AudioClip[] backgroundMusic;  
    private AudioSource backgroundMusicSource; 

    [Header("Sound Effects")]
    public AudioClip[] soundEffects; 
    private AudioSource sfxSource; 

    private void Awake()
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

        backgroundMusicSource = gameObject.AddComponent<AudioSource>();
        sfxSource = gameObject.AddComponent<AudioSource>();

        backgroundMusicSource.loop = true; 
        sfxSource.loop = false; 
    }

    public void PlayBackgroundMusic(int index)
    {
        if (index >= 0 && index < backgroundMusic.Length)
        {
            backgroundMusicSource.clip = backgroundMusic[index];
            backgroundMusicSource.Play();
            Debug.Log("Playing background music: " + backgroundMusic[index].name);
        }
        else
        {
            Debug.LogError("Invalid background music index.");
        }
    }

    public void StopBackgroundMusic()
    {
        backgroundMusicSource.Stop();
    }

    // Function to play sound effects
    public void PlaySoundEffect(int index)
    {
        if (index >= 0 && index < soundEffects.Length)
        {
            sfxSource.PlayOneShot(soundEffects[index]);
            Debug.Log("Playing sound effect: " + soundEffects[index].name);
        }
        else
        {
            Debug.LogError("Invalid sound effect index.");
        }
    }
}
