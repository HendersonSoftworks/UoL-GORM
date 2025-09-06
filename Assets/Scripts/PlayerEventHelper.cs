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
    [SerializeField]
    private AudioClip walkClip;
    [SerializeField]
    private AudioClip swingClip;
    [SerializeField]
    private AudioClip hurtClip;

    private void Start()
    {
        movementController = GetComponentInParent<PlayerMovementController>();
        attackController = GetComponentInParent<PlayerAttackController>();
        animationController = GetComponentInParent<PlayerAnimationController>();
    }

    public void EventSetIsAttackingFlagFalse()
    {
        animationController.playerAnimator.SetBool("isAttacking", false);
    }

    public void EventSetIsAttackingFlag(bool value)
    {
        animationController.playerAnimator.SetBool("isAttacking", value);
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
        attackController.FinishAttack();
    }

    public void EventPlayWalkClip()
    {
        GetComponentInParent<AudioSource>().PlayOneShot(walkClip, 2);
    }

    public void EventPlaySwingClip()
    {
        GetComponentInParent<AudioSource>().PlayOneShot(swingClip, 3);
    }

    public void EventPlayHurtClip()
    {
        GetComponentInParent<AudioSource>().PlayOneShot(hurtClip, 3);
    }
}
