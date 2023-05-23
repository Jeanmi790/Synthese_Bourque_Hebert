using TMPro;
using UnityEngine;

public class ManagerUIEndScene : MonoBehaviour
{
    [Header("TxtGameInfo")]
    [SerializeField] private TMP_Text txtScore = default;

    [SerializeField] private TMP_Text txtTime = default;

    [Header("TxtBestScore")]
    [SerializeField] private TMP_Text txtBestScore = default;

    [SerializeField] private TMP_Text txtBestTime = default;
    [SerializeField] private TMP_Text txtBestName = default;

    [Header("InputData")]
    [SerializeField] private GameObject inputPanel = default;

    [SerializeField] private TMP_InputField inputName = default;

    private readonly float defaultValue = 0f;
    private readonly string defaultName = "Player1";

    // Start is called before the first frame update
    private void Start()
    {
        inputPanel.SetActive(false);

        txtBestScore.text = PlayerPrefs.HasKey("BestScore") ? PlayerPrefs.GetFloat("BestScore").ToString() : defaultValue.ToString();
        txtBestName.text = PlayerPrefs.HasKey("BestPlayer") ? PlayerPrefs.GetFloat("BestTime").ToString("00") + " sec" : defaultValue.ToString();
        txtBestTime.text = PlayerPrefs.HasKey("BestTime") ? PlayerPrefs.GetString("BestPlayer") : defaultName;
        txtScore.text = PlayerPrefs.HasKey("Score") ? PlayerPrefs.GetFloat("Score").ToString() : defaultValue.ToString();
        txtTime.text = PlayerPrefs.HasKey("Time") ? PlayerPrefs.GetFloat("Time").ToString("00") + " sec" : defaultValue.ToString();

        if (PlayerPrefs.GetFloat("Score") > PlayerPrefs.GetFloat("BestScore"))
        {
            inputPanel.SetActive(true);
        }
    }

    public void UpdateBestScore()
    {
        if (PlayerPrefs.HasKey("Score") && PlayerPrefs.HasKey("Time"))
        {
            PlayerPrefs.SetFloat("BestScore", PlayerPrefs.GetFloat("Score"));
            PlayerPrefs.SetFloat("BestTime", PlayerPrefs.GetFloat("Time"));
            PlayerPrefs.SetString("BestPlayer", inputName.text);
            PlayerPrefs.Save();
        }
    }

    public void Refresh()
    {
        txtBestScore.text = PlayerPrefs.GetFloat("BestScore").ToString();
        txtBestTime.text = PlayerPrefs.GetFloat("BestTime").ToString("00") + " sec";
        txtBestName.text = PlayerPrefs.GetString("BestPlayer");
        inputPanel.SetActive(false);
    }
}