using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Animations.Rigging;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour, IDamageable
{
    public float maxHealth = 100f;
    public float currentHealth;
    public Transform playerTransform;
    
    private int  enemyIdleAnimation, enemyWalkAnimation; 
    private Animator animator;
    private NavMeshAgent agent;

    private Ragdoll ragdoll;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        ragdoll = GetComponent<Ragdoll>();
        currentHealth = maxHealth;
        var rigidBodies = GetComponentsInChildren<Rigidbody>();
        foreach (var rigidBody  in rigidBodies)
        {
            HitBox hitBox = rigidBody.gameObject.AddComponent<HitBox>();
            hitBox.enemyController = this;
        }   
    }

    // Update is called once per frame
    void Update()
    {
        agent.destination = playerTransform.position;
        animator.SetFloat("Speed", agent.velocity.magnitude);
    }
    private void Awake()
    {
        animator = GetComponent<Animator>();
        enemyIdleAnimation = Animator.StringToHash("Idle");
        enemyWalkAnimation = Animator.StringToHash("Walk");
        
    }

    public void TakeDamage(float damage, Vector3 direction)
    {
        currentHealth -= damage;
        if (currentHealth<=0)
        {
            Die();
        }
    }

    public void Die()
    {
        ragdoll.ActivateRagdoll();

    }
}
