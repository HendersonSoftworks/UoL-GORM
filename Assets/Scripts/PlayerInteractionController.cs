using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractionController : MonoBehaviour
{
    public PlayerInput playerInput;
    public InputAction interactionAction;

    [SerializeField]
    private PlayerCharacter playerCharacter;
    [SerializeField]
    private GameManager gameManager;

    private void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();

        playerInput = GetComponent<PlayerInput>();
        playerCharacter = GetComponent<PlayerCharacter>();

        interactionAction = playerInput.actions["Interact"];
    }

    private void Update()
    {
        if (gameManager.isGamePaused) { return; }

        ChestManageInteraction();
    }

    private void ChestManageInteraction()
    {
        if (!playerCharacter.inContactWithChest){ return; }
        if (playerCharacter.currentChest == null) { return; }
        if (playerCharacter.currentChest.GetComponent<Chest>().empty) { return; }

        float _interactionValue = interactionAction.ReadValue<float>();
        if (playerCharacter.inContactWithChest && _interactionValue == 1 &&
            playerCharacter.inContactWithChest == true)
        {
            gameManager.ShowChestChoice();
            playerCharacter.inContactWithChest = false;
        }
    }
}
