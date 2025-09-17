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
    private PlayerAudioManager playerAudio;
    [SerializeField]
    private AudioClip walkClip;
    [SerializeField]
    private AudioClip castClip;


    private void Start()
    {
        movementController = GetComponentInParent<PlayerMovementController>();
        attackController = GetComponentInParent<PlayerAttackController>();
        animationController = GetComponentInParent<PlayerAnimationController>();
        playerAudio = GetComponentInParent<PlayerAudioManager>();
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

    public void EventCastInvokeAttackCastSpell()
    {
        attackController.AttackCastSpell();
    }

    public void ResetCasting()
    {
        attackController.isCasting = false;
        movementController.canMove = true;
        animationController.SetCastAnim(false);
    }

    public void EventPlayWalkClip()
    {
        GetComponentInParent<AudioSource>().PlayOneShot(walkClip, AudioGlobalConfig.volEffects * AudioGlobalConfig.volScale);
    }

    public void EventPlaySwingClip()
    {
        playerAudio.PlaySwingClip();        
    }

    public void EventPlayCastClip()
    {
        GetComponentInParent<AudioSource>().PlayOneShot(castClip, AudioGlobalConfig.volEffects * AudioGlobalConfig.volScale);
    }
}
