using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerUI_StartScene : MonoBehaviour
{
[Header("InstructionPanel")]
    [SerializeField] GameObject instructionPanel = default;
    [Header("OptionPanel")]
    [SerializeField] GameObject optionPanel = default;
    // Start is called before the first frame update
    void Start()
    {
        instructionPanel.SetActive(false);
        optionPanel.SetActive(false);
    }

    public void openInstructionPanel() { instructionPanel.SetActive(true); }
    public void closeInstructionPanel() { instructionPanel.SetActive(false); }
    public void openOptionPanel() { optionPanel.SetActive(true); }
    public void closeOptionPanel() { optionPanel.SetActive(false); }
    

}
