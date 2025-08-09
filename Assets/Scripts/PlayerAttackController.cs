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

    [Header("Logic - Do not change")]
    public bool isAttacking = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        movementController = GetComponentInParent<PlayerMovementController>();
        animationController = GetComponent<PlayerAnimationController>();

        attackAction = playerInput.actions["Attack"];

    }

    // Update is called once per frame
    void Update()
    {
        ManageAttacking();
    }

    private void ManageAttacking()
    {
        if (isAttacking) { return; }
        float attackValue = attackAction.ReadValue<float>();
        if (attackValue == 0) { return; }
        
        isAttacking = true; // reset from eventhelper
        StartAttack();
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
