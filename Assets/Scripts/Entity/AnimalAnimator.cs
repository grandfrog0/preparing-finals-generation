using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalAnimator : MonoBehaviour
{
    private Animator _animator;
    private void Start()
    {
        _animator = GetComponent<Animator>();

        Animal animal = GetComponent<Animal>();
        animal.OnMove.AddListener(SetMove);
        animal.OnAttack.AddListener(OnAttack);
        animal.OnDamaged.AddListener(OnDamaged);
        animal.OnDead.AddListener(OnDeath);
    }

    public void SetMove(bool value)
    {
        _animator.SetBool("Move", value);
    }
    public void OnAttack()
    {
        _animator.SetTrigger("Attack");
    }
    public void OnDamaged()
    {
        _animator.SetTrigger("Damaged");
    }
    public void OnDeath()
    {
        _animator.SetTrigger("Death");
    }
}
