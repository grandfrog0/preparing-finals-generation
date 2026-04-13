using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgressiveAnimalAI : MonoBehaviour
{
    public float SeeRadius { get; set; }
    private Animal _animal;

    private void Start()
    {
        _animal = GetComponent<Animal>();

        SeeRadius = 20;
        GetComponent<SphereCollider>().radius = SeeRadius;

        InitAnimal();
    }

    private void OnTriggerStay(Collider other)
    {
        if (!_animal.InCooldown && other.CompareTag("Player") && other.TryGetComponent(out IHurtable hurtable))
        {
            if (Vector3.Distance(transform.position, other.transform.position) < _animal.AttackRange)
            {
                _animal.Attack(hurtable);
            }
            else
            {
                _animal.MoveToPoint(other.transform.position);
            }
        }
    }

    private void InitAnimal()
    {
        _animal.MaxHealth = 100;

        _animal.AttackDamage = 1;
        _animal.AttackCD = 1;
        _animal.AttackRange = 2.5f;

        _animal.Initialize();

        _animal.MoveSpeed = 5;
    }
}
