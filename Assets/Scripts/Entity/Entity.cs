using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Entity : MonoBehaviour, IHurtable
{
    public UnityEvent OnDamaged { get; } = new UnityEvent();
    public UnityEvent OnDead { get; } = new UnityEvent();

    public float MaxHealth { get; set; }
    public float Health { get; protected set; }
    public bool IsAttackAvailable { get; protected set; }

    public virtual void Initialize()
    {
        Health = MaxHealth;

        IsAttackAvailable = true;
        OnDead.AddListener(() => IsAttackAvailable = false);
    }

    public void TakeDamage(float value)
    {
        if (Health - value <= 0)
        {
            Health = 0;
            OnDead.Invoke();
            return;
        }

        Health -= value;
        OnDamaged.Invoke();
    }
}
