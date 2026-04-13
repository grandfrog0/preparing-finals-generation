using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHurtable
{
    bool IsAttackAvailable { get; }
    void TakeDamage(float value);
}
