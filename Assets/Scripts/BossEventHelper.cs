using UnityEngine;

public class BossEventHelper : MonoBehaviour
{
    [Header("Setup - Loaded on start")]
    [SerializeField]
    private BossController bossController;
    [SerializeField]
    private EnemySoundManager enemySound;

    private void Start()
    {
        bossController = GetComponentInParent<BossController>();
        enemySound = GetComponentInParent<EnemySoundManager>();
    }

    public void EventBossFireRandSpell()
    {
        enemySound.PlayEnemyCastClip();

        var randInt = Random.Range(0, bossController.spells.Length);
        bossController.FireRandSpell(randInt);
    }

    public void EventBossTeleport()
    {
        bossController.TeleportToRandomPoint();
    }
}
