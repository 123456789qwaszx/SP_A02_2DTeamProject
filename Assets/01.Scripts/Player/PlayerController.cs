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

    public SkillBook sklilbook;

    private void Awake()
    {
        GameManager.Instance.controller = this;
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        sklilbook = GetComponent<SkillBook>();

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

        // 기본적으로는 이동 입력에 따라 좌우 판단하지만, 공격 시 마우스 클릭에 의한 방향 전환
        // 가 있을 경우 그 값이 덮어씌워질 수 있으므로, 조건에 따라 분리할 수 있습니다.
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

        // 공격 애니메이션 트리거 설정
        if (inputVector.magnitude > 0.1f)
            animator.SetTrigger("Run_Attack");
        else
            animator.SetTrigger("Idle_Attack");

        // 마우스 위치를 스크린 좌표에서 월드 좌표로 변환
        Vector3 ms = Mouse.current.position.ReadValue();
        // 카메라와 플레이어 간의 Z 거리 차이를 활용하여 월드 좌표 계산
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(new Vector3(ms.x, ms.y, Mathf.Abs(Camera.main.transform.position.z - transform.position.z)));
        mouseWorld.z = 0f;

        // 여기에 플레이어의 좌우 방향 전환 로직 추가
        // 만약 마우스가 플레이어 기준 오른쪽에 있으면 오른쪽, 왼쪽에 있으면 왼쪽으로 바라보게 설정
        if (mouseWorld.x > transform.position.x)
        {
            // 마우스가 오른쪽에 있으면 flipX = true (즉, 오른쪽을 바라보도록)
            spriteRenderer.flipX = true;
        }
        else
        {
            // 마우스가 왼쪽에 있으면 flipX = false (즉, 왼쪽을 바라보도록)
            spriteRenderer.flipX = false;
        }

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