using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("MainMenuMusic")]
    [SerializeField] AudioSource mainMenuMusic = default;
    [Header("MainSceneMusic")]
    [SerializeField] AudioSource mainSceneMusic = default;

    void Start()
    {
        mainMenuMusic = GetComponent<AudioSource>();
        mainMenuMusic.volume = 0.3f;
        mainSceneMusic = GetComponent<AudioSource>();
        mainSceneMusic.volume = 0.3f;
    }

    public void playMainMenuMusic(AudioClip audioClip)
    {
        mainMenuMusic.PlayOneShot(audioClip);
    }

    public void muteMainMenuMusic()
    {
        mainMenuMusic.mute = true;
    }

    public int getMainMenuMusicVolume()
    {
        return (int)(mainMenuMusic.volume * 100);
    }

    public void setMainMenuMusicVolume(int volume)
    {
        mainMenuMusic.volume = volume / 100f;
    }

    public void stopMainMenuMusic()
    {
        mainMenuMusic.Stop();
    }

    //Mettre dans une autre classe?

    public void playMainSceneMusic(AudioClip audioClip)
    {
        mainSceneMusic.PlayOneShot(audioClip);
    }

    public void muteMainSceneMusic()
    {
        mainSceneMusic.mute = true;
    }

    public int getMainSceneMusicVolume()
    {
        return (int)(mainSceneMusic.volume * 100);
    }

    public void setMainSceneMusicVolume(int volume)
    {
        mainSceneMusic.volume = volume / 100f;
    }
    
    public void stopMainSceneMusic()
    {
        mainSceneMusic.Stop();
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
