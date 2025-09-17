using UnityEngine;

public class EnemyEventHelper : MonoBehaviour
{
    [Header("Setup - Loaded on start")]
    [SerializeField]
    private EnemyMovementController movementController;
    [SerializeField]
    private EnemyAttackController attackController;
    [SerializeField]
    private EnemySoundManager enemySound;

    private void Start()
    {
        movementController = GetComponentInParent<EnemyMovementController>();
        attackController = GetComponentInParent<EnemyAttackController>();
        enemySound = GetComponentInParent<EnemySoundManager>();
    }

    public void EventSetSpeedToAttackMoveSpeed()
    {
        movementController.moveSpeed = attackController.attackMoveSpeed;
    }

    public void EventAttackTarget()
    {
        // Attack stuff - animation, hitbox activiation, etc
        print(gameObject.name + " attacking " + movementController.targetObject.name);
    }

    public void EventFinishAttack()
    {
        // Set movement state to chase at end of attack anim
        attackController.attackState = EnemyAttackController.AttackStates.cooldown;
        movementController.moveSpeed = movementController.normalSpeed;
    }

    public void EventPlayAttackClip()
    {
        enemySound.PlayEnemyAttackClip();
    }
}
