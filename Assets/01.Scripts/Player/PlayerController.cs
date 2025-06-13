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

    private Warrior warrior;
    private float attackCooldown = 0f; // 남은 쿨타임 시간

    public WarriorAttackController attackController;
    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        player = GetComponent<Player>();
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>(); // Body 안에서 찾아짐

        warrior = GetComponentInChildren<Warrior>();
        if (warrior == null)
        {
            Debug.LogError("Warrior 스크립트를 찾을 수 없습니다.");
        }

        player = warrior != null ? warrior : GetComponent<Player>();
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

        if (attackCooldown > 0f)
            attackCooldown -= Time.fixedDeltaTime;

        GameManager.Instance.MoveDir = inputVector;
    }

    private void OnAttackPerformed(InputAction.CallbackContext context)
    {
        if (attackCooldown > 0f || warrior == null)
            return;

        // 공격 애니메이션 실행
        if (inputVector.magnitude > 0.1f)
            animator.SetTrigger("Run_Attack");
        else
            animator.SetTrigger("Idle_Attack");

        // 쿨타임 설정 (초당 공격 횟수 기준 → 간격은 1 / 속도)
        float delay = 1f / warrior.AttackSpeed;
        attackCooldown = delay;

        // 1) 마우스 스크린→월드 좌표 변환
        Vector3 ms = Mouse.current.position.ReadValue();
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(ms);
        mouseWorld.z = 0f;

        // 2) 공격 컨트롤러 호출
        attackController.AttackByMouse(mouseWorld);

    }
}
