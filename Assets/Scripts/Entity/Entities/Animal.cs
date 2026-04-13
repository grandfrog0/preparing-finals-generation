using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class Animal : Entity, IAttacker, IPointMovable
{
    public UnityEvent<bool> OnMove { get; } = new ();
    public UnityEvent OnAttack { get; } = new ();

    public float AttackDamage { get; set; }
    public float AttackRange { get; set; }
    public float AttackCD { get; set; }
    public float MoveSpeed
    { 
        get => _agent.speed;
        set => _agent.speed = value;
    }
    public bool InCooldown { get; private set; }

    private NavMeshAgent _agent;
    private Coroutine _moveRoutine;

    public override void Initialize()
    {
        base.Initialize();
        _agent = GetComponent<NavMeshAgent>();
        InCooldown = false;
    }

    public void Attack(IHurtable hurtable, float damage)
    {
        if (InCooldown)
            return;

        hurtable.TakeDamage(damage);

        OnAttack.Invoke();
        StartCoroutine(CooldownRoutine());
    }
    public void Attack(IHurtable hurtable)
    {
        if (InCooldown)
            return;

        Attack(hurtable, AttackDamage);
    }
    public void MoveToPoint(Vector3 pos)
    {
        _agent.destination = pos;

        OnMove.Invoke(true);
        
        if (_moveRoutine == null)
        {
            _moveRoutine = StartCoroutine(MoveRoutine());
        }
    }

    private IEnumerator CooldownRoutine()
    {
        InCooldown = true;
        yield return new WaitForSeconds(AttackCD);
        InCooldown = false;
    }
    private IEnumerator MoveRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);

        while (true)
        {
            if (Vector3.Distance(transform.position, _agent.destination) < 1)
            {
                OnMove.Invoke(false);
            }
            yield return wait;
        }
    }
}
