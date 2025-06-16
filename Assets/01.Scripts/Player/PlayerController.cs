using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction attackAction;

    private Rigidbody rb;
    private Player player; // 기본 Player 클래스 (공통 필드 보유)
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private Vector2 inputVector;
    private float attackCooldown = 0f;

    [Header("전투 관련 컴포넌트")]
    public WarriorAttackController warriorAttackController;
    public ArcherAttackController archerAttackController;
    public WizardAttackController wizardAttackController;

    private Warrior warrior;
    private Archer archer;
    private Wizard wizard;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        // 공통 Player 컴포넌트 가져오기
        player = GetComponent<Player>();

        // 직업 구분
        warrior = GetComponentInChildren<Warrior>();
        archer = GetComponentInChildren<Archer>();
        wizard = GetComponentInChildren<Wizard>();

        if (warrior != null)
        {
            player = warrior;
        }
        else if (archer != null)
        {
            player = archer;
        }
        else if (wizard != null)
        {
            player = wizard;
        }
        else
        {
            Debug.LogError("플레이어 직업 스크립트를 찾을 수 없습니다.");
        }
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

        animator.SetFloat("Run", inputVector.magnitude);

        // 방향 처리
        if (inputVector.x > 0.01f)
            spriteRenderer.flipX = true;
        else if (inputVector.x < -0.01f)
            spriteRenderer.flipX = false;

        if (attackCooldown > 0f)
            attackCooldown -= Time.fixedDeltaTime;

        GameManager.Instance.MoveDir = inputVector;
    }

    private void OnAttackPerformed(InputAction.CallbackContext context)
    {
        if (attackCooldown > 0f || player == null)
            return;

        if (inputVector.magnitude > 0.1f)
            animator.SetTrigger("Run_Attack");
        else
            animator.SetTrigger("Idle_Attack");

        Vector3 ms = Mouse.current.position.ReadValue();
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(ms);
        mouseWorld.z = 0f;

        // 직업별 공격 처리
        if (warrior != null && warriorAttackController != null)
        {
            float delay = 1f / warrior.AttackSpeed;
            attackCooldown = delay;
            warriorAttackController.AttackByMouse(mouseWorld);
        }
        else if (archer != null && archerAttackController != null)
        {
            float delay = 1f / archer.AttackSpeed;
            attackCooldown = delay;
            archerAttackController.ShootArrow(mouseWorld);
        }
        else if (wizard != null && wizardAttackController != null)
        {
            float delay = 1f / wizard.AttackSpeed;
            attackCooldown = delay;
            wizardAttackController.CastSpell(mouseWorld);
        }
    }
}
