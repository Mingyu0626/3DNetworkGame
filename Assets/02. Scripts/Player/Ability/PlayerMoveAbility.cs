using Photon.Pun;
using UnityEngine;

public class PlayerMoveAbility : PlayerAbility
{
    // 누적될 속력 변수
    private float _yVelocity = 0f;

    private Vector3 _receivedPosition = Vector3.zero;
    private Quaternion _receivedRotation = Quaternion.identity;

    private PlayerStaminaAbility _playerStaminaAbility;
    private MinimapCamera _minimapCamera;

    protected override void Awake()
    {
        base.Awake();
        _playerStaminaAbility = Owner.GetAbility<PlayerStaminaAbility>();
    }

    private void Start()
    {
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void DoAbility()
    {
        Move();
        Run();
    }

    private void Move()
    {
        if (Owner.PlayerState == EPlayerState.Death)
        {
            return;
        }
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
        CharacterController.Move(dir * Owner.Stat.CurrentMoveSpeed * Time.deltaTime);
    }

    private void ApplyGravity()
    {
        _yVelocity += Owner.Stat.Gravity * Time.deltaTime;
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && CharacterController.isGrounded &&
            Owner.Stat.JumpStaminaCost < Owner.Stat.CurrentStamina)
        {
            _yVelocity = Owner.Stat.JumpPower;
            Owner.Stat.CurrentStamina -= Owner.Stat.JumpStaminaCost;
        }
    }

    private void Run()
    {
        if (Owner == null || Owner.Stat == null || CharacterController == null)
        {
            return;
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (0f < Owner.Stat.CurrentStamina)
            {
                Animator.SetBool("Run", true);
                Owner.Stat.CurrentMoveSpeed = Owner.Stat.RunSpeed;
                Owner.Stat.CurrentStamina -= Owner.Stat.RunStaminaCostPerSecond * Time.deltaTime;
                _playerStaminaAbility.IsUsingStamina = true;
            }
            else
            {
                Animator.SetBool("Run", false);
                Owner.Stat.CurrentMoveSpeed = Owner.Stat.MoveSpeed;
                _playerStaminaAbility.IsUsingStamina = false;
            }
        }
        else
        {
            Animator.SetBool("Run", false);
            Owner.Stat.CurrentMoveSpeed = Owner.Stat.MoveSpeed;
            _playerStaminaAbility.IsUsingStamina = false;
        }
    }
}