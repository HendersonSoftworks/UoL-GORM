using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System;

public class MainMenuController : MonoBehaviour
{
    public PlayerInput playerInput;
    public InputAction submitAction;

    [SerializeField]
    private MainMenuAudio mainMenuAudio;
    [SerializeField]
    private GameObject subtitleText;
    [SerializeField]
    private GameObject continueText;
    [SerializeField]
    private GameObject choicePanel;
    [SerializeField]
    private Button startButton;
    [SerializeField]
    private GameObject introPanel;
    [SerializeField]
    private Animator canvasAnimator;
    [SerializeField]
    private GameObject SettingsPanel;

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        submitAction = playerInput.actions["Submit"];
        Time.timeScale = 1;
    }

    void Update()
    {
        if (canvasAnimator.GetBool("InTitleScreen") &&
            submitAction.triggered && 
            continueText.GetComponent<TextMeshProUGUI>().enabled)
        {
            TransitionToMainMenu();
        }
    }

    internal void SetInTitleScreen()
    {
        canvasAnimator.SetBool("InTitleScreen", true);
    }

    private void TransitionToMainMenu()
    {
        canvasAnimator.SetBool("InMainMenu", true);

        mainMenuAudio.PlayMenuConfirmOneshot();

        continueText.GetComponent<TextMeshProUGUI>().enabled = false;
        subtitleText.GetComponent<TextMeshProUGUI>().enabled = false;
        choicePanel.SetActive(true);
        startButton.Select();
    }

    public void TrainsitionToDungeon()
    {
        mainMenuAudio.PlayMenuConfirmOneshot();
        mainMenuAudio.PlayScaryLaughOneshot();
        canvasAnimator.SetBool("StartGame", true);
    }

    public void LoadDungeon()
    {
        SceneManager.LoadScene("dungeon");
    }

    public void OpenSettingsPanel()
    {
        choicePanel.SetActive(false);
        SettingsPanel.SetActive(true);

        mainMenuAudio.PlayMenuConfirmOneshot();
    }

    public void CloseSettingsPanel()
    {
        choicePanel.SetActive(true);
        SettingsPanel.SetActive(false);

        mainMenuAudio.PlayMenuConfirmOneshot();
    }
}
