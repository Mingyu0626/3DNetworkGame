using Photon.Pun;
using UnityEngine;

public class PlayerAttackAbility : PlayerAbility
{
    private float AttackTimer = 0f;

    public Collider WeaponCollider;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        DeActiveCollider();
    }

    // 위치 및 회전처럼 상시 확인이 필요한 데이터 동기화 : IPunObservable(OnPhotonSerializeView)
    // 트리거, 공격, 피격과 같은 간헐적으로 특정 이벤트가 발생했을 때의 변화된 데이터 동기화는
    // "RPC(Remote Procedure Call, 원격 함수 호출)"를 이용한다.
    // 물리적으로 떨어져 있는 다른 디바이스의 함수를 호출하는 기능이다.
    // RPC 함수를 호출하면, 네트워크를 통해 다른 사용자의 스크립트에서 해당 함수가 호출된다.

    protected override void Update()
    {
        base.Update();
    }

    protected override void DoAbility()
    {
        Attack();
    }

    private void Attack()
    {
        AttackTimer += Time.deltaTime;
        if (Input.GetMouseButton(0) && (1f / Owner.Stat.AttackSpeed) <= AttackTimer &&
            Owner.Stat.AttackStaminaCost < Owner.Stat.CurrentStamina)
        {
            AttackTimer = 0f;
            Owner.Stat.CurrentStamina -= Owner.Stat.AttackStaminaCost;

            // RPC 메서드 호출 방식
            PhotonView.RPC(nameof(PlayAttackAnimation), RpcTarget.All, Random.Range(1, 4));
        }
    }

    public void ActiveCollider()
    {
        WeaponCollider.enabled = true;
    }

    public void DeActiveCollider()
    {
        WeaponCollider.enabled = false;
    }

    // RPC로 호출할 메서드는 반드시 [PunRPC] 어트리뷰트를 메서드 앞에 명시해줘야 한다.
    [PunRPC]
    private void PlayAttackAnimation(int randomNumber)
    {
        Animator.SetTrigger($"Attack{randomNumber}");
    }

    public void Hit(Collider other)
    {
        if (!PhotonView.IsMine)
        {
            return;
        }
        if (other.GetComponent<IDamaged>() == null)
        {
            return;
        }
        DeActiveCollider();
        PhotonView otherPhotonView = other.gameObject.GetComponent<PhotonView>();
        otherPhotonView.RPC("Damaged", RpcTarget.All, Owner.Stat.Damage);
    }
}