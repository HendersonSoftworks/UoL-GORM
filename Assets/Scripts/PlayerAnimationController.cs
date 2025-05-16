using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    public Animator playerAnimator;

    private PlayerMovementController playerMovementController;

    private void Start()
    {
        playerMovementController = GetComponent<PlayerMovementController>();
    }

    private void Update()
    {
        EvaluateAnimationState(playerMovementController);
    }

    public void EvaluateAnimationState(PlayerMovementController _playerMovementController)
    {
        if (_playerMovementController.moveInput != Vector2.zero) { SetRunningAnim(true); }
        else { SetRunningAnim(false);}
    }

    public void SetRunningAnim(bool value)
    {
        playerAnimator.SetBool("isRunning", value);
    }
}
