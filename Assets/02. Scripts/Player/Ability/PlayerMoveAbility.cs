using Photon.Pun;
using UnityEngine;

public class PlayerMoveAbility : PlayerAbility, IPunObservable
{
    // 누적될 속력 변수
    private float _yVelocity = 0f;

    private Vector3 _receivedPosition = Vector3.zero;
    private Quaternion _receivedRotation = Quaternion.identity;

    private MinimapCamera _minimapCamera;

    protected override void Awake()
    {
        base.Awake();
        _minimapCamera = FindFirstObjectByType<MinimapCamera>();
        if (_minimapCamera != null && PhotonView.IsMine)
        {
            _minimapCamera.SetTarget(transform);
        }
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
        if (!PhotonView.IsMine)
        {
            transform.position = Vector3.Lerp(transform.position, _receivedPosition, Time.deltaTime * 20f);
            transform.rotation = Quaternion.Slerp(transform.rotation, _receivedRotation, Time.deltaTime * 20f);
        }
        // Jump();
        Movement();
        Run();
    }

    // 데이터 동기화를 위한 데이터 전송 및 수신 기능
    // stream : 서버에서 주고받을 데이터가 담겨있는 변수
    // info : 송수신 성공/실패 여부에 대한 로그
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // 데이터를 전송하는 상황 -> 데이터를 보내주면 되고
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }

        // 데이터를 수신하는 상황 -> 받은 데이터를 세팅하면 된다.
        else if (stream.IsReading)
        {
            _receivedPosition = (Vector3)stream.ReceiveNext();
            _receivedRotation = (Quaternion)stream.ReceiveNext();
        }
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
        var playerStaminaAbility = Owner.GetAbility<PlayerStaminaAbility>();
        if (Owner == null || Owner.Stat == null || CharacterController == null)
        {
            return;
        }

        if (Input.GetKey(KeyCode.LeftShift) && CharacterController.isGrounded)
        {
            if (0f < Owner.Stat.CurrentStamina)
            {
                Owner.Stat.CurrentMoveSpeed = Owner.Stat.RunSpeed;
                Owner.Stat.CurrentStamina -= Owner.Stat.RunStaminaCostPerSecond * Time.deltaTime;
                playerStaminaAbility.IsUsingStamina = true;
            }
            else
            {
                Owner.Stat.CurrentMoveSpeed = Owner.Stat.MoveSpeed;
                playerStaminaAbility.IsUsingStamina = false;
            }
        }
        else
        {
            Owner.Stat.CurrentMoveSpeed = Owner.Stat.MoveSpeed;
            playerStaminaAbility.IsUsingStamina = false;
        }
    }
}