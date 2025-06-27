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
        Attack();
    }

    private void Attack()
    {
        AttackTimer += Time.deltaTime;
        if (Owner.Stat.AttackCooltime <= AttackTimer && Input.GetMouseButton(0))
        {
            Debug.Log("Attack");
            AttackTimer = 0f;
            Animator.SetTrigger($"Attack{Random.Range(1, 4)}");
        }
    }
}