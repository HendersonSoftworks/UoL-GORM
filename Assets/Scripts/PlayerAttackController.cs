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
    public int currentSpellSlotSelected = 0;
    public Spell currentSpellSelected;

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

        Invoke("SetInitialSpellGraphics", 2);
    }

    void Update()
    {
        if (gameManager.isGamePaused) { return; }

        ManageDefending();
        ManageAttacking();
        ManageCasting();
    }

    private void SetInitialSpellGraphics()
    {
        if (playerCharacter.spells[0] != null)
        {
            // Set currently selected to first spell slot
            gameManager.uiManager.SetCurrentSpellSlotSelected(currentSpellSlotSelected);
            currentSpellSelected = playerCharacter.spells[currentSpellSlotSelected];
        }
    }

    private void ManageCasting()
    {
        if (isDefending) { return; }
        if (isAttacking) { return; }
        if (playerCharacter.spells[0] == null) { return; }

        // Spell slot 1
        float selectSpell1Value = selectSpell1.ReadValue<float>();
        //if (selectSpell1Value == 0) { return; }
        if (selectSpell1Value == 1)
        {
            currentSpellSlotSelected = 0;
        }

        // Spell slot 2
        float selectSpell2Value = selectSpell2.ReadValue<float>();
        //if (selectSpell2Value == 0) { return; }
        if (selectSpell2Value == 1)
        {
            currentSpellSlotSelected = 1;
        }

        // Spell slot 3
        float selectSpell3Value = selectSpell3.ReadValue<float>();
        //if (selectSpell3Value == 0) { return; }
        if (selectSpell3Value == 1)
        {
            currentSpellSlotSelected = 2;
        }

        // Set Spell slot
        gameManager.uiManager.SetCurrentSpellSlotSelected(currentSpellSlotSelected);
        currentSpellSelected = playerCharacter.spells[currentSpellSlotSelected];

        if (castAction.WasPressedThisFrame())
        {
            if (currentSpellSelected == null) { return; }
            
            if (playerCharacter.currentMana < currentSpellSelected.castValue)
            {
                print("Not enough mana");
                return;
            }

            isCasting = true;
            movementController.canMove = false;
            movementController.SetVelToZero();
            animationController.SetRunningAnim(false);
            animationController.SetCastAnim(true);
        }
    }

    public void AttackCastSpell()
    {
        if (playerCharacter.spells[currentSpellSlotSelected] == null) { return; }

        playerCharacter.spells[currentSpellSlotSelected].Cast(GetPlayerCharacter(), transform.up);
        currentSpellSelected = playerCharacter.spells[currentSpellSlotSelected];
    }

    private Character GetPlayerCharacter()
    {
        playerCharacter = GetComponent<PlayerCharacter>();

        return playerCharacter;
    }

    private void ManageDefending()
    {
        //if (!movementController.canMove) { return; }
        if (isAttacking) { return; }
        if (isCasting) { return; }

        float blockValue = blockAction.ReadValue<float>();

        if (blockValue == 0) { FinishDefend(); ; }
        else if (blockValue == 1) { StartDefend(); }
    }

    private void ManageAttacking()
    {
        if (isDefending) { return; }
        if (isAttacking) { return; }
        if (isCasting) { return; }

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
