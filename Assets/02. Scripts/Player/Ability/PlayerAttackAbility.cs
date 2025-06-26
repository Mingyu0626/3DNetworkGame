using UnityEngine;

public class PlayerAttackAbility : PlayerAbility
{
    private float AttackTimer = 0f;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Update()
    {
        AttackTimer += Time.deltaTime;
        Attack();
    }

    private void Attack()
    {
        if (AttackTimer >= Owner.Stat.ATTACK_COOLTIME && Input.GetMouseButton(0))
        {
            AttackTimer = 0f;
            Animator.SetTrigger($"Attack{Random.Range(1, 4)}");
        }
    }
}