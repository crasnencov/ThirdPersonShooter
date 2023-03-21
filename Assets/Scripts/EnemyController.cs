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
    public float blinkIntensity = 10f;
    public float blinkDuration = 0.1f;
    public Material bodyMaterial;

    private float blinkTimer;


    private int enemyIdleAnimation, enemyWalkAnimation;
    private Animator animator;
    private NavMeshAgent agent;

    private Ragdoll ragdoll;

    Renderer[] rend;

    private int takingDamage;
    [SerializeField] private float animationPlayTransition = 0.15f;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        ragdoll = GetComponent<Ragdoll>();
        currentHealth = maxHealth;
        var rigidBodies = GetComponentsInChildren<Rigidbody>();
        foreach (var rigidBody in rigidBodies)
        {
            HitBox hitBox = rigidBody.gameObject.AddComponent<HitBox>();
            hitBox.enemyController = this;
        }

        // rend = GetComponent<Renderer> ();
        rend = GetComponentsInChildren<Renderer>();
        takingDamage = Animator.StringToHash("Taking Damage");
    }

    // Update is called once per frame
    void Update()
    {
        agent.destination = playerTransform.position;
        animator.SetFloat("Speed", agent.velocity.magnitude);

        // blinkTimer -= Time.deltaTime;
        // float lerp = Mathf.Clamp01(blinkTimer / blinkDuration);
        // float intensity = (lerp * blinkIntensity) + 10.0f;
        // foreach (var t in rend)
        // {
        //     t.material.color = new Color(217,217,217);
        // }
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
        if (currentHealth <= 0)
        {
            Die();
        }

        // blinkTimer = blinkDuration;
        // foreach (var t in rend)
        // {
        //     t.material.color =  Color.red;
        // }
        // animator.CrossFade(takingDamage, animationPlayTransition);
    }

    public void Die()
    {
        ragdoll.ActivateRagdoll();
    }
}