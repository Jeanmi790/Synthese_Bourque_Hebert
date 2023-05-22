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
        GamePanel(OptionPanel, false);

        GamePanel(PausePanel, false);
        GamePanel(GameOverPanel, false);
        GamePanel(InstructionPanel, false);
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

    private void GamePanel(GameObject panel, bool status)
    {
        panel.SetActive(status);
    }

    public void Instruction()
    {
        if (!InstructionPanel.activeSelf)
        {
            GamePanel(InstructionPanel, true);
        }
        else
        {
            GamePanel(InstructionPanel, false);
        }
    }

    public void Option()
    {
        if (!OptionPanel.activeSelf)
        {
            GamePanel(OptionPanel, true);
        }
        else
        {
            GamePanel(OptionPanel, false);
        }
    }

    public void PauseGame()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isPaused && !gameInfo.IsPlayerDead)
        {
            Time.timeScale = 0;
            GamePanel(PausePanel, true);
            isPaused = true;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && isPaused && !gameInfo.IsPlayerDead)
        {
            Time.timeScale = 1;
            GamePanel(PausePanel, false);
            isPaused = !true;
        }
    }

    public void Resume()
    {
        Time.timeScale = 1;
        GamePanel(PausePanel, false);
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
        GamePanel(GameOverPanel, true);
    }

    public void ResetGame()
    {
        gameInfo.RestartGame();
        Time.timeScale = 1;
        GamePanel(GameOverPanel, false);
        GamePanel(PausePanel, false);
        isPaused = false;
    }
}