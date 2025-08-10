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

    [Header("Logic - Do not change")]
    public bool isAttacking = false;
    public bool isDefending = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        movementController = GetComponentInParent<PlayerMovementController>();
        animationController = GetComponent<PlayerAnimationController>();

        attackAction = playerInput.actions["Attack"];
        blockAction = playerInput.actions["Block"];
    }

    // Update is called once per frame
    void Update()
    {
        ManageDefending();
        ManageAttacking();
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
