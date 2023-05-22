using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("MusicSelected")]
    [SerializeField] private AudioSource MusicSelected = default;

    [Header("DisplayVolume")]
    [SerializeField] private TMP_Text DisplayVolume;

    [SerializeField] private Slider volumeSlider;

    [Header("ButtonSound")]
    [SerializeField] private GameObject[] buttonPlay = default;

    [SerializeField] private GameObject[] buttonPause = default;
    [SerializeField] private GameObject[] buttonMute = default;
    [SerializeField] private GameObject[] buttonUnmute = default;

    private void Start()
    {
        MusicSelected = GetComponent<AudioSource>();
        MusicSelected.volume = 0.15f;
        DisplayVolume.text = MusicSelected.volume.ToString("0.0");
        volumeSlider.value = MusicSelected.volume;

        SetButtonStatus(buttonPlay, false);
        SetButtonStatus(buttonPause,true );
        SetButtonStatus(buttonMute, true);
        SetButtonStatus(buttonUnmute, false);
    }

    private void Update()
    {
        if(volumeSlider != null)
        {
            DisplayVolume.text = (MusicSelected.volume * 100f).ToString("0") + "%";
        }
       
    }

    public void playMusicSelected()
    {
        MusicSelected.Play();
        SetButtonStatus(buttonPlay, false);
        SetButtonStatus(buttonPause, true);
    }

    public void pauseMusicSelected()
    {
        MusicSelected.Pause();
        SetButtonStatus(buttonPlay, true);
        SetButtonStatus(buttonPause, false);
    }

    public void muteMusicSelected()
    {
        MusicSelected.mute = true;
        SetButtonStatus(buttonMute, false);
        SetButtonStatus(buttonUnmute, true);
    }

    public void unmuteMusicSelected()
    {
        MusicSelected.mute = false;
        SetButtonStatus(buttonMute, true);
        SetButtonStatus(buttonUnmute, false);
    }

    public float getMusicSelectedVolume()
    {
        return MusicSelected.volume;
    }

    public void setMusicSelectedVolume(float volume)
    {
        MusicSelected.volume = volume;
    }

    public void stopMusicSelected()
    {
        MusicSelected.Stop();
    }

    private void SetButtonStatus(GameObject[] button, bool status)
    {
        foreach (GameObject buttonStatus in button)
        {
            buttonStatus.SetActive(status);
        }
    }
}