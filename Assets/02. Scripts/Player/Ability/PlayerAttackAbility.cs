using UnityEngine;

public class PlayerAttackAbility : PlayerAbility
{
    private float AttackTimer = 0f;

    protected override void Awake()
    {
        base.Awake();
    }

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
        if (Input.GetMouseButton(0) && (1f / Owner.Stat.AttackSpeed) <= AttackTimer)
        {
            AttackTimer = 0f;
            Animator.SetTrigger($"Attack{Random.Range(1, 4)}");
        }
    }
}