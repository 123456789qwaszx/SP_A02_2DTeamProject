using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction attackAction;

    private Rigidbody rb;
    private Player player;
    private Animator animator;
    private SpriteRenderer spriteRenderer; // Body의 스프라이트 렌더러

    private Vector2 inputVector;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        player = GetComponent<Player>();
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>(); // Body 안에서 찾아짐
    }

    private void OnEnable()
    {
        moveAction = playerInput.actions["Move"];
        attackAction = playerInput.actions["Attack"];
        attackAction.performed += OnAttackPerformed;
    }

    private void OnDisable()
    {
        attackAction.performed -= OnAttackPerformed;
    }

    private void FixedUpdate()
    {
        inputVector = moveAction.ReadValue<Vector2>();
        Vector2 move = inputVector * player.MoveSpeed * Time.fixedDeltaTime;
        player.Rb.MovePosition(player.Rb.position + move);

        // 애니메이션 갱신
        animator.SetFloat("Run", inputVector.magnitude);

        // 오른쪽 이동 중이면 flip, 왼쪽이면 원래 방향
        if (inputVector.x > 0.01f)
            spriteRenderer.flipX = true;   // 오른쪽 → 반전
        else if (inputVector.x < -0.01f)
            spriteRenderer.flipX = false;  // 왼쪽 → 기본
    }

    private void OnAttackPerformed(InputAction.CallbackContext context)
    {
        if (inputVector.magnitude > 0.1f)
            animator.SetTrigger("Run_Attack");
        else
            animator.SetTrigger("Idle_Attack");
    }
}
