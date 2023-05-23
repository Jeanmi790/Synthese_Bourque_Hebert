using UnityEngine;

public class ManagerUI_StartScene : MonoBehaviour
{
    [Header("InstructionPanel")]
    [SerializeField] private GameObject instructionPanel = default;

    [Header("OptionPanel")]
    [SerializeField] private GameObject optionPanel = default;

    // Start is called before the first frame update
    private void Start()
    {
        instructionPanel.SetActive(false);
        optionPanel.SetActive(false);
    }

    public void openPanel(GameObject panel)
    { panel.SetActive(true); }

    public void closePanel(GameObject panel)
    { panel.SetActive(false); }
}