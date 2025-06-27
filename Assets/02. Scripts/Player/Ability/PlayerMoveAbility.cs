using Unity.Android.Gradle.Manifest;
using UnityEngine;

public class PlayerMoveAbility : PlayerAbility
{
    private CharacterController _characterController;

    // 누적될 속력 변수
    private float _yVelocity = 0f;

    protected override void Awake()
    {
        base.Awake();
    }

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

        Animator.SetFloat("Move", dir.magnitude);

        if (dir.magnitude <= 0f)
        {
            return;
        }

        // 카메라가 바라보는 방향 기준으로 수정하기
        dir = Camera.main.transform.TransformDirection(dir);
        // 2-2. 수직 속도에 중력 값을 적용한다.
        ApplyGravity();
        dir.y = _yVelocity;

        // 2-3. 수직 속도에 캐릭터 점프 여부에 따른 값을 적용한다.
        Jump();

        // 3. 이동 속도에 따라 그 방향으로 이동하기
        // 캐릭터의 위치 = 현재 위치 + 속도  * 시간
        _characterController.Move(dir * Owner.Stat.MoveSpeed * Time.deltaTime);
    }

    private void ApplyGravity()
    {
        _yVelocity += Owner.Stat.Gravity * Time.deltaTime;
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _characterController.isGrounded)
        {
            _yVelocity = Owner.Stat.JumpPower;
        }
    }
}