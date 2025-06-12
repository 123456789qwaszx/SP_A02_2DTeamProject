using UnityEngine;
using UnityEngine.InputSystem;

// PlayerInput 컴포넌트를 요구하도록 지정
[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    private PlayerInput playerInput;            // PlayerInput: 인풋 시스템을 처리할 객체
    private InputAction moveAction;             // 이동에 사용할 InputAction

    private Rigidbody rb;                       // Rigidbody를 통해 이동 처리

    private Player player;                      // Player 스크립트 참조

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>(); 
        player = GetComponent<Player>();
    }

    // 오브젝트가 활성화될 때 InputAction 연결
    private void OnEnable()
    {
        moveAction = playerInput.actions["Move"]; // "Move" 액션 불러오기
    }

    // 고정된 간격으로 물리 기반 이동 처리
    private void FixedUpdate()
    {

        Vector2 inputVector = moveAction.ReadValue<Vector2>();                    // 이동 입력값 가져오기 (Vector2: x = 좌우, y = 상하)
        Vector2 move = inputVector * player.MoveSpeed * Time.fixedDeltaTime;      // 입력값을 2D 
        player.Rb.MovePosition(player.Rb.position + move);                        // Rigidbody를 사용한 이동 (충돌 감지 가능)
    }
}
