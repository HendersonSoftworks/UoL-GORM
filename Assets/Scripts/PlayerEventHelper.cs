using UnityEngine;

public class PlayerEventHelper : MonoBehaviour
{
    [Header("Setup - Loaded on start")]
    [SerializeField]
    private PlayerMovementController movementController;
    [SerializeField]
    private PlayerAttackController attackController;
    [SerializeField]
    private PlayerAnimationController animationController;

    private void Start()
    {
        movementController = GetComponentInParent<PlayerMovementController>();
        attackController = GetComponentInParent<PlayerAttackController>();
        animationController = GetComponent<PlayerAnimationController>();
    }

    public void EventSetIsAttackingFlag()
    {
        animationController.playerAnimator.SetBool("isAttacking", false);
    }

    public void EventSetSpeedToAttackMoveSpeed()
    {
        //movementController.moveSpeed = attackController.attackMoveSpeed;
    }

    public void EventAttackTarget()
    {
        // Attack stuff - animation, hitbox activiation, etc
        //print(gameObject.name + " attacking " + movementController.targetObject.name);
    }

    public void EventFinishAttack()
    {
        // Set movement state to chase at end of attack anim
        //attackController.attackState = EnemyAttackController.AttackStates.cooldown;
        //movementController.moveSpeed = movementController.normalSpeed;
    }
}
