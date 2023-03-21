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
    public Transform patrolRoute;
    public List<Transform> locations;
    public Transform player;
    public float dieForce;
    private float blinkTimer;


    private int enemyIdleAnimation, enemyWalkAnimation;
    private Animator animator;
    private NavMeshAgent agent;
    private UIHealthBar healthBar;
    private Ragdoll ragdoll;

    Renderer[] rend;

    private int takingDamage;
    [SerializeField] private float animationPlayTransition = 0.15f;

    private int locationIndex = 0;


    private void Awake()
    {
        animator = GetComponent<Animator>();
        enemyIdleAnimation = Animator.StringToHash("Idle");
        enemyWalkAnimation = Animator.StringToHash("Walk");
    }

    void Start()
    {
        InitializePatrolRoute();
        Debug.Log(locationIndex);
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

        MoveToNextLocation();
        Debug.Log("move to next start");
        player = GameObject.Find("Player").transform;
    }


    void Update()
    {
        //agent.destination = playerTransform.position;
        animator.SetFloat("Speed", agent.velocity.magnitude);
        // MoveToNextLocation();
        if (agent.remainingDistance < 0.02f && !agent.pathPending)
        {
            MoveToNextLocation();
            Debug.Log("move to next update");
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
            Debug.Log("return");
            return;
        }

        // agent.destination = locations[locationIndex].position;
        locationIndex = (locationIndex + 1) % locations.Count;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            Debug.Log("Player detected - attack!");
            agent.destination = player.position;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Player")
        {
            Debug.Log("Player out of range, resume patrol");
        }
    }
}