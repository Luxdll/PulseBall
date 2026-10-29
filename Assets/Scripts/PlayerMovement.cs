using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float acceleration = 20f;
    [SerializeField] private float speedMultiplier = 1f;
    [SerializeField] private float boostTimeRemaining;
    [SerializeField] private VirtualJoystick joystick;

    private Rigidbody2D body;
    private Vector2 moveInput;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        UpdateSpeedBoost();
        moveInput = Vector2.zero;

        Keyboard keyboard = Keyboard.current;

        if (keyboard != null)
        {
            if (keyboard.aKey.isPressed || keyboard.leftArrowKey.isPressed)
                moveInput.x -= 1f;

            if (keyboard.dKey.isPressed || keyboard.rightArrowKey.isPressed)
                moveInput.x += 1f;

            if (keyboard.sKey.isPressed || keyboard.downArrowKey.isPressed)
                moveInput.y -= 1f;

            if (keyboard.wKey.isPressed || keyboard.upArrowKey.isPressed)
                moveInput.y += 1f;
        }

        if (joystick != null && joystick.Direction.sqrMagnitude > 0f)
        {
            moveInput = joystick.Direction;
        }

        moveInput = Vector2.ClampMagnitude(moveInput, 1f);
    }

    private void FixedUpdate()
    {
        Vector2 targetVelocity = moveInput * moveSpeed * speedMultiplier;

        body.linearVelocity = Vector2.MoveTowards(
            body.linearVelocity,
            targetVelocity,
            acceleration * Time.fixedDeltaTime);
    }

    public void ApplySpeedBoost(float multiplier, float duration)
    {
        if (multiplier <= 1f || duration <= 0f)
            return;

        speedMultiplier = multiplier;
        boostTimeRemaining = duration;
    }

    private void UpdateSpeedBoost()
    {
        if (boostTimeRemaining <= 0f)
            return;

        boostTimeRemaining = Mathf.Max(
            0f, boostTimeRemaining - Time.deltaTime);

        if (boostTimeRemaining == 0f)
        {
            speedMultiplier = 1f;
        }
    }
}