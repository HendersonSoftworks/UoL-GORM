using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerUIController : MonoBehaviour
{
    private GameManager gameManager;
    private PlayerInput playerInput;
    private InputAction pauseAction;
    private InputAction openStatsAction;
    private InputAction openSpellsAction;

    void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
        playerInput = GetComponent<PlayerInput>();
        pauseAction = playerInput.actions["Pause"];
        openStatsAction = playerInput.actions["Stats"];
        openSpellsAction = playerInput.actions["Spells"];
    }

    private void Update()
    {
        if (gameManager.uiManager.floorStartPanel.activeInHierarchy)
        {
            return;
        }

        MenuManager();
    }

    private void MenuManager()
    {
        // Pause
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

        // Stats
        if (openStatsAction.WasPressedThisFrame())
        {
            if (gameManager.uiManager.statsPanel.activeInHierarchy)
            {
                gameManager.CloseStatsScreen();
            }
            else
            {
                gameManager.OpenStatsScreen();
            }
        }

        // Spells
        if (openSpellsAction.WasPressedThisFrame())
        {
            if (gameManager.uiManager.spellsPanel.activeInHierarchy)
            {
                gameManager.CloseSpellScreen();
            }
            else
            {
                gameManager.OpenSpellScreen();
            }
        }

    }
}
