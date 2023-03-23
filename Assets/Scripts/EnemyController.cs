using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
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
    public Transform patrolRoute;
    public List<Transform> locations;
    private PlayerController playerController;

    public float dieForce;
    private float blinkTimer;


    private int enemyIdleAnimation, enemyWalkAnimation;
    private Animator animator;
    private NavMeshAgent agent;
    private UIHealthBar healthBar;
    private Ragdoll ragdoll;

    Renderer[] rend;

    private int takingDamage, attack;
    [SerializeField] private float animationPlayTransition = 0.15f;

    private int locationIndex = 0;
    private bool isDead = false;
    private float timer = 0.0f;
    public float maxTime = 1.0f;
    public float maxDistanceToPlayer = 5f;
    private bool isChasingPlayer = false;

    public int damage = 10;
    public float attackSpeed = 1.5f;
    private bool canAttack = false;
    private float timeOfLastAttack;
    private bool countAsKilled = false;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        enemyIdleAnimation = Animator.StringToHash("Idle");
        enemyWalkAnimation = Animator.StringToHash("Walk");
    }

    void Start()
    {
        InitializePatrolRoute();
        
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        ragdoll = GetComponent<Ragdoll>();
        healthBar = GetComponentInChildren<UIHealthBar>();

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
        attack = Animator.StringToHash("Attack");

        // MoveToNextLocation();
    }


    void Update()
    {
        //agent.destination = playerTransform.position;

        if (!isChasingPlayer)
        {
            animator.SetFloat("Speed", agent.velocity.magnitude);
            agent.speed = 2;
        }
        else
        {
            animator.SetFloat("Speed", 5);
            agent.speed = 5;
        }


        if (!isDead && agent.remainingDistance < agent.stoppingDistance && !agent.pathPending)
        {
            MoveToNextLocation();
        }

        var distance = Vector3.Distance(transform.position, playerTransform.position);

        if (!isDead && distance < maxDistanceToPlayer)
        {
            MoveToPLayer();
        }
        else
        {
            isChasingPlayer = false;
        }
    }

    public void TakeDamage(float damage, Vector3 direction)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die(direction);
        }

        healthBar.SetHealthBarPercentage(currentHealth / maxHealth);
        // animator.CrossFade(takingDamage, animationPlayTransition);
    }

    public void Die(Vector3 direction)
    {
        direction.y = 1;
        ragdoll.ActivateRagdoll();
        healthBar.gameObject.SetActive(false);
        ragdoll.ApplyForce(direction * dieForce);
        gameObject.GetComponent<NavMeshAgent>().enabled = false;
        isDead = true;
        
        if (!countAsKilled)
        {
            GameManager.Instance.EnemyKilled();
            countAsKilled = true;
        }
        
    }

    private void InitializePatrolRoute()
    {
        foreach (Transform child in patrolRoute)
        {
            locations.Add(child);
        }
    }

    private void MoveToNextLocation()
    {
        if (locations.Count == 0)
        {
            return;
        }

        agent.destination = locations[locationIndex].position;
        locationIndex = (locationIndex + 1) % locations.Count;
    }

    private void MoveToPLayer()
    {
        isChasingPlayer = true;

        var distance = Vector3.Distance(transform.position, playerTransform.position);
        if (distance <= maxDistanceToPlayer)
        {
            agent.destination = playerTransform.position;
            // animator.SetFloat("Speed", 0f, 0.3f, Time.deltaTime);
            isChasingPlayer = false;
            canAttack = true;
            if (Time.time >= timeOfLastAttack + attackSpeed)
            {
                timeOfLastAttack = Time.time;
                Attack();
            }
        }
        else
        {
            // agent.speed = 5;
            // animator.SetFloat("Speed", 5f, 0.3f, Time.deltaTime);
        }
    }

    private void Attack()
    {
        animator.CrossFade(attack, animationPlayTransition);
        playerController.PlayerTakeDamage(damage);
        if (playerController.playerIsDead)
        {
            Debug.Log("player dead" );
            MoveToNextLocation();
        }
    }
}