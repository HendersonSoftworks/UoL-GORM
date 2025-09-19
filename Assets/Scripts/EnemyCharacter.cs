using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharacter : Character
{
    [Header("Loaded on startup")]
    [SerializeField]
    private EnemyMovementController movementController;
    [SerializeField]
    private EnemySoundManager soundManager;

    [SerializeField]
    private float pushDuration;

    private void Start()
    {
        movementController = GetComponent<EnemyMovementController>();
        soundManager = GetComponent<EnemySoundManager>();

        currentWeapon = weapons[0];
    }

    public void PushEnemyAway(Collider2D collider, float force = 200f, float pushDuration = 0.5f)
    {
        if (movementController == null) { return; }

        Vector2 forceAngle = (transform.position - collider.GetComponentInParent<Transform>().position).normalized;

        movementController.beingPushed = true;
        movementController.rb2D.AddForce(forceAngle * force);
        StartCoroutine(StopEnemyPush(pushDuration));
    }

    private IEnumerator StopEnemyPush(float timer)
    {
        yield return new WaitForSeconds(timer);
        movementController.beingPushed = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Damage enemy if hit by player attack
        if (collision.tag == "pHitbox")
        {
            PushEnemyAway(collision);
            DamageCharacter(collision.GetComponentInParent<PlayerCharacter>(), this);

            soundManager.PlayEnemyHurtClip();
        }

        // Damage enemy if hit by spell
        if (collision.tag == "spell")
        {
            PushEnemyAway(collision);
            DamageCharacter(
                collision.GetComponent<Spell>().casterCharacter, 
                this, 
                collision.GetComponent<Spell>());
        }
    }
}
