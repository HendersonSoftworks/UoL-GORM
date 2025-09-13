using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerUIController : MonoBehaviour
{
    private GameManager gameManager;
    private PlayerInput playerInput;
    private InputAction pauseAction;

    void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
        playerInput = GetComponent<PlayerInput>();
        pauseAction = playerInput.actions["Pause"];
    }

    private void Update()
    {
        PauseMenuManager();
    }

    private void PauseMenuManager()
    {
        if (pauseAction.WasPressedThisFrame())
        {
            if (gameManager.uiManager.PauseMenuPanel.activeInHierarchy)
            {
                gameManager.ClosePauseMenu();
            }
            else
            {
                gameManager.OpenPauseMenu();
            }
        }
    }
}
