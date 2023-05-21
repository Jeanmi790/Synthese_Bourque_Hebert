using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("MusicSelected")]
    [SerializeField] private AudioSource MusicSelected = default;

    [Header("DisplayVolume")]
    [SerializeField] private TMP_Text DisplayVolume = default;

    [SerializeField] private Slider volumeSlider = default;

    private void Start()
    {
        MusicSelected = GetComponent<AudioSource>();
        MusicSelected.volume = 0.15f;
        DisplayVolume.text = MusicSelected.volume.ToString("0.0");
        volumeSlider.value = MusicSelected.volume;
    }

    private void Update()
    {
        DisplayVolume.text = (MusicSelected.volume * 100f).ToString("0") + "%";
    }

    public void playMusicSelected()
    {
        MusicSelected.Play();
    }

    public void pauseMusicSelected()
    {
        MusicSelected.Pause();
    }

    public void muteMusicSelected()
    {
        MusicSelected.mute = true;
    }

    public void unmuteMusicSelected()
    {
        MusicSelected.mute = false;
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
}