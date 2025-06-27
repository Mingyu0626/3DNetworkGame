using Unity.Cinemachine;
using UnityEngine;

public class PlayerRotateAbility : PlayerAbility
{
    [SerializeField]
    private Transform _cameraRoot;

    private float _mx;
    private float _my;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        CinemachineCamera camera = GameObject.FindWithTag("FollowCamera")
            .GetComponent<CinemachineCamera>();
        camera.Follow = _cameraRoot;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        _mx += mouseX * Owner.Stat.RotateSpeed * Time.deltaTime;
        _my += mouseY * Owner.Stat.RotateSpeed * Time.deltaTime;

        _my = Mathf.Clamp(_my, Owner.Stat.YRotationMin, Owner.Stat.YRotationMax);

        transform.eulerAngles = new Vector3(0, _mx, 0.0f);
        _cameraRoot.localEulerAngles = new Vector3(-_my, 0f, 0.0f);
    }
}