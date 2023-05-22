using TMPro;
using UnityEngine;

public class ManagerUI : MonoBehaviour
{
    [Header("OptionPanel")]
    [SerializeField] private GameObject OptionPanel = default;

    [Header("PausePanel")]
    [SerializeField] private GameObject PausePanel = default;

    [Header("GameOverPanel")]
    [SerializeField] private GameObject GameOverPanel = default;

    [Header("InstructionPanel")]
    [SerializeField] private GameObject InstructionPanel = default;

    [Header("TxtActualTime")]
    [SerializeField] private TMP_Text TxtActualTime = default;

    [Header("TxtActualScore")]
    [SerializeField] private TMP_Text TxtActualScore = default;

    private bool isPaused;

    private IGameInfo gameInfo;


    // Start is called before the first frame update
    private void Start()
    {
        OptionPanel.SetActive(false);
        PausePanel.SetActive(false);
        GameOverPanel.SetActive(false);
        InstructionPanel.SetActive(false);
        isPaused = false;
        gameInfo = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    private void Update()
    {
        PauseGame();
        UpdateGameInfo();
        PanelGameOver();
    }

    public void GamePanel(GameObject panel)
    {
        if (!panel.activeSelf)
        {
            panel.SetActive(true);
        }
        else
        {
            panel.SetActive(false);
        }
    }

    public void PauseGame()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isPaused && !gameInfo.IsPlayerDead)
        {
            Time.timeScale = 0;
            GamePanel(PausePanel);
            isPaused = true;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && isPaused && !gameInfo.IsPlayerDead)
        {
            Time.timeScale = 1;
            GamePanel(PausePanel);
            OptionPanel.SetActive(false);
            GameOverPanel.SetActive(false);
            InstructionPanel.SetActive(false);
            isPaused = !true;
        }
    }

    public void Resume()
    {
        Time.timeScale = 1;
        GamePanel(PausePanel);
        isPaused = !true;
    }

    private void UpdateGameInfo()
    {
        TxtActualTime.text = gameInfo.InGameTime.ToString("00") + " sec";
        TxtActualScore.text = gameInfo.Score.ToString();
    }

    public void PanelGameOver()
    {
        if (!gameInfo.IsPlayerDead) { return; }
        GameOverPanel.SetActive(true);
    }

    public void ResetGame()
    {
        gameInfo.RestartGame();
        Time.timeScale = 1;
        GamePanel(GameOverPanel);
        GamePanel(PausePanel);
        isPaused = false;
    }
}