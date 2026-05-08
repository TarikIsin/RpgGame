using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Entity
{
    [Header("Movement")]
    [SerializeField] protected float moveSpeed = 8f;
    [SerializeField] private float jumpForce = 12f;
    private bool canJump = true;
    private float moveInput;

    override protected void Update()
    {
        base.Update();
        HandleInput();
    }
    private void HandleInput()
    {
        float left = Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed ? -1f : 0f;
        float right = Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed ? 1f : 0f;
        moveInput = left + right;

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
            TryToJump();

        if (Mouse.current.leftButton.wasPressedThisFrame)
            HandleAttack();
    }

    protected override void HandleAnimations()
    {
        base.HandleAnimations();

        animator.SetFloat("yVelocity", rb.linearVelocity.y);
        animator.SetBool("isGrounded", isGrounded);
    }
    protected override void HandleMovement()
    {
        if (canMove)
            rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
        else
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
    }

    private void TryToJump()
    {
        if (isGrounded && canJump)
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }

    public override void EnableMovement(bool enable)
    {
        base.EnableMovement(enable);
        canJump = enable;
    }

    protected override void OnHealthChanged(int currentHealth)
    {
        UI.Instance.UpdateCharacterHealth(currentHealth);
    }

    protected override void Die()
    {
        base.Die();
        UI.Instance.EnableGameOverUI();
    }
}
