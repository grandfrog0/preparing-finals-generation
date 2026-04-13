using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEntity : Entity
{
    public void Start()
    {
        MaxHealth = 100;
        OnDamaged.AddListener(() => Debug.Log(("Player damaged!", Health)));
        Initialize();
    }
}
