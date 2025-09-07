using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttackController : MonoBehaviour
{
    [Header("Setup - Loaded on start")]
    [SerializeField]
    private PlayerMovementController movementController;
    [SerializeField]
    private PlayerAnimationController animationController;
    public PlayerInput playerInput;
    public InputAction attackAction;
    public InputAction blockAction;
    public InputAction castAction;
    public InputAction selectSpell1;
    public InputAction selectSpell2;
    public InputAction selectSpell3;
    public int currentSpellSelected = 0;

    [SerializeField]
    private PlayerCharacter playerCharacter;
    [SerializeField]
    private GameManager gameManager;

    [Header("Logic - Do not change")]
    public bool isAttacking = false;
    public bool isDefending = false;
    public bool isCasting = false;

    private void Awake()
    {
        gameManager = FindFirstObjectByType<GameManager>();        
    }

    void Start()
    {
        movementController = GetComponentInParent<PlayerMovementController>();
        animationController = GetComponent<PlayerAnimationController>();
        playerCharacter = GetComponent<PlayerCharacter>();

        attackAction = playerInput.actions["Attack"];
        blockAction = playerInput.actions["Block"];
        castAction = playerInput.actions["Cast"];
        selectSpell1 = playerInput.actions["SelectSpell1"];
        selectSpell2 = playerInput.actions["SelectSpell2"];
        selectSpell3 = playerInput.actions["SelectSpell3"];
    }

    void Update()
    {
        if (gameManager.isGamePaused) { return; }

        ManageDefending();
        ManageAttacking();
        ManageCasting();
    }

    private void ManageCasting()
    {
        // Spell slot 1
        float selectSpell1Value = selectSpell1.ReadValue<float>();
        if (selectSpell1Value == 0) { return; }
        if (selectSpell1Value == 1)
        {
            currentSpellSelected = 0;
        }

        // Spell slot 2
        float selectSpell2Value = selectSpell2.ReadValue<float>();
        if (selectSpell2Value == 0) { return; }
        if (selectSpell2Value == 1)
        {
            currentSpellSelected = 1;
        }

        // Spell slot 3
        float selectSpell3Value = selectSpell3.ReadValue<float>();
        if (selectSpell3Value == 0) { return; }
        if (selectSpell3Value == 1)
        {
            currentSpellSelected = 2;
        }

        // Set Spell slot
        gameManager.uiManager.SetCurrentSpellSlotSelected(currentSpellSelected);
    }

    private void ManageDefending()
    {
        //if (!movementController.canMove) { return; }
        if (isAttacking) { return; }
        
        float blockValue = blockAction.ReadValue<float>();

        if (blockValue == 0) { FinishDefend(); ; }
        else if (blockValue == 1) { StartDefend(); }
    }

    private void ManageAttacking()
    {
        if (isDefending) { return; }
        if (isAttacking) { return; }

        float attackValue = attackAction.ReadValue<float>();
        if (attackValue == 0) { return; }

        if (playerCharacter.currentWeapon.staminaCost > playerCharacter.currentStamina) 
        {
            print("TODO show low stamina!");
            return; 
        }

        playerCharacter.currentStamina -= playerCharacter.currentWeapon.staminaCost;

        isAttacking = true; // reset from eventhelper
        StartAttack();
    }

    public void StartDefend()
    {
        animationController.playerAnimator.SetBool("isDefending", true);
        movementController.SetVelToZero();
        movementController.moveSpeed = movementController.attackMoveSpeed;
        isDefending = true;
    }

    public void FinishDefend()
    {
        animationController.playerAnimator.SetBool("isDefending", false);
        movementController.moveSpeed = movementController.baseSpeed;
        isDefending = false;
    }

    public void StartAttack()
    {
        animationController.playerAnimator.SetBool("isAttacking", true);
        movementController.SetVelToZero();
        movementController.canMove = false;
        movementController.moveSpeed = movementController.attackMoveSpeed;
    }
    
    public void FinishAttack()
    {
        isAttacking = false;
        movementController.canMove = true;
        movementController.moveSpeed = movementController.baseSpeed;
    }
}
