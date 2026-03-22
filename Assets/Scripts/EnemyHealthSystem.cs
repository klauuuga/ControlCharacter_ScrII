using System;
using UnityEngine;

public class EnemyHealthSystem : MonoBehaviour
{
    private Rigidbody[] bones;
    private float health;
    private void Start()
    {
        bones = GetComponentsInChildren<Rigidbody>();
        foreach (var bone in bones)
        {
            bone.isKinematic = true; //pedra no se cae
        }
    }

    public void TakeTotalDamage(float damageToApply)
    {
        health = health - damageToApply;
        if (health <= 0)
        {
            Destroy(GetComponent<Animator>()); //destruye el componente cuando se muere
            //2. pongo los huesos en kinematic a false
            bones = GetComponentsInChildren<Rigidbody>();
            foreach (var bone in bones)
            {
                bone.isKinematic = false; //pedra no se cae
            }
        }
    }
}
