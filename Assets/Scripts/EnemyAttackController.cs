using UnityEngine;

public class EnemyAttackController : MonoBehaviour
{
    public enum AttackStates { ready, warmup, attacking, cooldown };

    [Header("Setup - Set in UI")]
    public BoxCollider2D hitbox;
    [SerializeField]
    private float hitboxTimer;
    [SerializeField]
    private float hitboxTimerReset;

    [Header("Setup - Loaded on start")]
    [SerializeField]
    private EnemyMovementController movementController;
    [SerializeField]
    private EnemyAnimationController animationController;

    [Header("Config - Enemy")]
    public AttackStates attackState = AttackStates.ready;
    [SerializeField]
    private float attackWarmupTimer;
    private float attackWarmupTimerReset;
    [SerializeField]
    private float attackCooldownTimer;
    private float attackCooldownTimerReset;
    [SerializeField]
    private float attackDist = 0;
    public float attackMoveSpeed;
    public Spell[] spells;
    
    #region System Methods

    void Start()
    {
        movementController = GetComponent<EnemyMovementController>();
        animationController = GetComponent<EnemyAnimationController>();

        attackWarmupTimerReset = attackWarmupTimer;
        attackCooldownTimerReset = attackCooldownTimer;
    }

    void Update()
    {
        ManageAttackState();
    }

    #endregion

    #region Public Methods

    #endregion

    #region Private Methods

    private void ManageAttackState()
    {
        switch (attackState)
        {
            case AttackStates.ready:
                ManageReadyToWarmup();
                break;

            case AttackStates.warmup:
                ManageWarmupToAttacking();

                break;
            case AttackStates.cooldown:
                ManageCooldownToReady();

                break;
            default:
                break;
        }
    }

    private void ManageReadyToWarmup()
    {
        // Switch to warming up if ready to attack and close to player
        if (movementController.targetObject == null) { return; }

        var distFromTarget = Vector2.Distance(movementController.targetObject.transform.position, transform.position);
        if (distFromTarget <= attackDist && attackState == AttackStates.ready)
        {
            attackState = AttackStates.warmup;
        }
    }

    private void ManageWarmupToAttacking()
    {
        //if (attackState == AttackStates.attacking) { return; }

        attackWarmupTimer -= Time.deltaTime;
        if (attackWarmupTimer <= 0)
        {
            attackWarmupTimer = attackWarmupTimerReset;
            attackState = AttackStates.attacking;
            //movementController.currentMoveState = EnemyMovementController.MoveStates.attacking; // breaking it
            animationController.SetState(EnemyMovementController.MoveStates.attacking);
        }
    }

    private void ManageCooldownToReady()
    {
        attackCooldownTimer -= Time.deltaTime;
        if (attackCooldownTimer <= 0)
        {
            attackCooldownTimer = attackCooldownTimerReset;
            attackState = AttackStates.ready;
        }
    }

    public void FireSpell()
    {
        var enemy = gameObject.GetComponent<EnemyCharacter>();
        spells[0].Cast(enemy, transform.up);
    }

    private void OnDrawGizmosSelected()
    {
        // Attack distance
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackDist);
    }

    #endregion
}
