using System;
using UnityEngine;

public class EnemyPart : MonoBehaviour
{
    [SerializeField] private float multiplier;

    private EnemyHealthSystem healthSystem;
    private void Awake()
    {
        healthSystem = transform.root.GetComponent<EnemyHealthSystem>();
    }

    public void TakeDamage(float baseDamage)
    {
        float damageToApply = baseDamage * multiplier;
        healthSystem.TakeTotalDamage(damageToApply);
        
        
    }
}
