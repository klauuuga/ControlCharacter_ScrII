using System;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;
    
    [SerializeField] private Transform targetToFollow;
    
    [Header ("Attack Behavior")]
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRadius;
    [SerializeField] private LayerMask whatIsDamagable;


    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        agent.SetDestination(targetToFollow.position);

        if (DestinationReached()) //si estas listo y ... PREGUNTA DE EXAMEN
        {
            LookToTarget();
            animator.SetBool("Reached", true);
            agent.isStopped = true; //Me aseguro de estar quieto mientras lanzo el ataque
        }
    }

    private void LookToTarget()
    {
        Vector3 direction = (targetToFollow.position - transform.position);
        direction.y = 0f;
        transform.rotation = Quaternion.LookRotation(direction); //le das una direccion y lo transforma en una rotacion
    }

    private void DoDamage()
    {
        Collider [] collisions = Physics.OverlapSphere(attackPoint.position, attackRadius, whatIsDamagable);

        foreach (var collider in collisions)
        {
           Debug.Log(collider.gameObject.name); 
        }
    }

    private bool DestinationReached()
    {
        return !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance;
    }

    //se ejecuta cuando se termine la animacion
    private void OnAttackFinished()
    {
        if (agent.remainingDistance > agent.stoppingDistance)
        {
            animator.SetBool("Reached", false);
            agent.isStopped = false;
        }

    }
}
