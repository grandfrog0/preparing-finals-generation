using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttacker
{
    float AttackDamage { get; }
    float AttackRange { get; }
    float AttackCD { get; }
    void Attack(IHurtable hurtable, float damage);
}
