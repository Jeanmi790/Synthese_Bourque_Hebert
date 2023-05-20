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

    private bool isPaused;

    // Start is called before the first frame update
    private void Start()
    {
        OptionPanel.SetActive(false);
        PausePanel.SetActive(false);
        GameOverPanel.SetActive(false);
        InstructionPanel.SetActive(false);
        isPaused = false;
    }

    // Update is called once per frame
    private void Update()
    {
        PauseGame();
    }

    public void OpenPanel(GameObject panel)
    {
        panel.SetActive(true);
    }

    public void ClosePanel(GameObject panel)
    {
        panel.SetActive(false);
    }

    private void PauseGame()
    {
        if (Input.GetKey(KeyCode.Escape) && !isPaused)
        {
            Time.timeScale = 0;
            OpenPanel(PausePanel);
            isPaused = true;
        }
        else if (Input.GetKey(KeyCode.Escape) && isPaused)
        {
            Time.timeScale = 1;
            ClosePanel(PausePanel);
            isPaused = !true;
        }
    }
}