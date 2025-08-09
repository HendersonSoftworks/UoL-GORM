using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharacter : Character
{
    [SerializeField]
    private float pushDuration;

    [SerializeField]
    private EnemyMovementController movementController;

    private void Start()
    {
        movementController = GetComponent<EnemyMovementController>();

        currentWeapon = weapons[0];
    }

    private void PushEnemyAway(Collider2D collider, float force = 200f, float pushDuration = 0.5f)
    {
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
        if (collision.tag == "pHitbox")
        {
            print("enemy oof!");
            PushEnemyAway(collision);
        }
    }
}