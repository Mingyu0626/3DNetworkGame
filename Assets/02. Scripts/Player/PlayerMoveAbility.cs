using Unity.Android.Gradle.Manifest;
using UnityEngine;

public class PlayerMoveAbility : MonoBehaviour
{
    [SerializeField]
    private PlayerData _playerData;

    private CharacterController _characterController;
    // 누적될 속력 변수
    private float _yVelocity = 0f;

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        Jump();
        Movement();
    }

    private void Movement()
    {
        // 목표: 키보드 [W], [A], [S], [D] 키를 누르면 캐릭터를 그 방향으로 이동시키고 싶다.
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        // 2. '캐릭터가 바라보는 방향'을 기준으로 방향 설정하기
        Vector3 dir = new Vector3(h, 0, v);
        dir = dir.normalized;

        // 2-2. 수직 속도에 중력 값을 적용한다.
        ApplyGravity();

        // 2-3. 수직 속도에 캐릭터 점프 여부에 따른 값을 적용한다.
        Jump();

        dir.y = _yVelocity;

        // 3. 이동 속도에 따라 그 방향으로 이동하기
        // 캐릭터의 위치 = 현재 위치 + 속도  * 시간
        _characterController.Move(dir * _playerData.MoveSpeed * Time.deltaTime);
    }

    private void ApplyGravity()
    {
        if (!_characterController.isGrounded)
        {
            _yVelocity += _playerData.Gravity * Time.deltaTime;
        }
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _characterController.isGrounded)
        {
            _yVelocity = _playerData.JumpPower;
        }
    }
}