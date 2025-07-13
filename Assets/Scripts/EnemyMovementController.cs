using System;
using UnityEngine;

public class EnemyMovementController : MonoBehaviour
{
    [Header("Setup - Leave empty")]
    [SerializeField]
    private GameObject targetObject;
    [SerializeField]
    private GameObject playerObject;
    [SerializeField]
    private bool targetIsInRange = false;

    void Start()
    {
        SetPlayerObject();
        SetTargetasPlayer();   
    }

    void Update()
    {
        ManageMovement();
    }

    private void ManageMovement()
    {
        DetectIfTargetInRange();
    }

    private void DetectIfTargetInRange()
    {
        
    }

    private void SetPlayerObject()
    {
        playerObject = FindAnyObjectByType<PlayerMovementController>().gameObject;
    }

    private void SetTargetasPlayer()
    {
        if (playerObject == null)
        {
            return;
        }

        targetObject = playerObject;
    }



}
