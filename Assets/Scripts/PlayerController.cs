using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController), typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    public float targetSpeed;
    public float maxPlayerHealth = 100;
    public float currentPlayerHealth;

    [SerializeField] private float moveSpeed = 2.0f;
    [SerializeField] private float runSpeed = 5.0f;
    [SerializeField] private float jumpHeight = 1.0f;
    [SerializeField] private float gravityValue = -9.81f;
    [SerializeField] private float rotationSpeed = 5f;


    [SerializeField] private float animationSmoothTime = 0.1f;
    [SerializeField] private float animationPlayTransition = 0.15f;
    [SerializeField] private Transform aimTarget;
    [SerializeField] private float aimDistance = 10f;

    private CharacterController controller;
    private PlayerInput playerInput;
    private GunSelector gunSelector;
    private Vector3 playerVelocity;
    private bool groundedPlayer;

    private InputAction moveAction, jumpAction, runAction;
    public static InputAction ShootAction, ReloadAction;
    private InputAction pistolWeaponAction, shotgunWeaponAction, machineGunWeaponAction;

    [SerializeField] private InputActionReference actionReference;

    private Transform cameraTransform;

    public Animator animator;
    private int moveXAnimationParameterId, moveZAnimationParameterId;
    private Vector2 currentAnimationBlendVector;
    private Vector2 animationVelocity;
    private int jumpAnimation, runAnimation, walkAnimation;
    private int pistolRecoilAnimation, shotgunRecoilAnimation, machinegunRecoilAnimation;

    public bool playerIsDead = false;
    private PlayerHealthBar healthBar;
    private GameManager gameManager;

    Ragdoll ragdoll;
    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        gunSelector = GetComponent<GunSelector>();

        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];
        ShootAction = playerInput.actions["Shoot"];
        ReloadAction = playerInput.actions["Reload"];
        runAction = playerInput.actions["Run"];
        pistolWeaponAction = playerInput.actions["Pistol"];
        shotgunWeaponAction = playerInput.actions["Shotgun"];
        machineGunWeaponAction = playerInput.actions["Machine Gun"];

        cameraTransform = Camera.main.transform;
        Cursor.lockState = CursorLockMode.Locked;

        //Animations
        animator = GetComponent<Animator>();
        moveXAnimationParameterId = Animator.StringToHash("MoveX");
        moveZAnimationParameterId = Animator.StringToHash("MoveZ");
        jumpAnimation = Animator.StringToHash("Pistol Jump");
        pistolRecoilAnimation = Animator.StringToHash("Pistol Shoot Recoil");
        shotgunRecoilAnimation = Animator.StringToHash("Shotgun Shoot Recoil");
        machinegunRecoilAnimation = Animator.StringToHash("Machinegun Shoot Recoil");

        runAnimation = Animator.StringToHash("Run");
        walkAnimation = Animator.StringToHash("Walk");
        ragdoll = GetComponent<Ragdoll>();
        
    }

    private void Start()
    {
        runAction.started += _ => { Run(); };
        runAction.canceled += _ => { Walk(); };
        gunSelector.SwitchGun("Pistol");
        currentPlayerHealth = maxPlayerHealth;
        healthBar = GetComponentInChildren<PlayerHealthBar>();
    }

    private void OnEnable()
    {
        // ShootAction.performed += _ => ShootGun();
        pistolWeaponAction.performed += _ => gunSelector.SwitchGun("Pistol");
        shotgunWeaponAction.performed += _ => gunSelector.SwitchGun("Shotgun");
        machineGunWeaponAction.performed += _ => gunSelector.SwitchGun("MachineGun");

        // actionReference.action.Enable();
    }

    private void OnDisable()
    {
        // ShootAction.performed -= _ => ShootGun();
        pistolWeaponAction.performed -= _ => gunSelector.SwitchGun("Pistol");
        shotgunWeaponAction.performed -= _ => gunSelector.SwitchGun("Shotgun");
        machineGunWeaponAction.performed -= _ => gunSelector.SwitchGun("MachineGun");
        actionReference.action.Disable();
    }


    void Update()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Vector2 input = moveAction.ReadValue<Vector2>();
        currentAnimationBlendVector = Vector2.SmoothDamp(currentAnimationBlendVector, input, ref animationVelocity,
            animationSmoothTime);
        Vector3 move = new Vector3(currentAnimationBlendVector.x, 0, currentAnimationBlendVector.y);
        move = move.x * cameraTransform.right.normalized + move.z * cameraTransform.forward.normalized;
        move.y = 0f;
        controller.Move(move * Time.deltaTime * targetSpeed);


        // Changes the height position of the player..
        if (jumpAction.triggered && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
            animator.CrossFade(jumpAnimation, animationPlayTransition);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        //Turn towards the camera
        float targetAngle = cameraTransform.eulerAngles.y;
        Quaternion targetRotation = Quaternion.Euler(0, targetAngle, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        animator.SetFloat(moveXAnimationParameterId, currentAnimationBlendVector.x);
        animator.SetFloat(moveZAnimationParameterId, currentAnimationBlendVector.y);

        aimTarget.position = cameraTransform.position + cameraTransform.forward * aimDistance;
        if(animator == null)
        {
            animator = GetComponent<Animator>();
        }
    }

    public void ShootGun()
    {
        animator.CrossFade(pistolRecoilAnimation, animationPlayTransition);
        animator.CrossFade(shotgunRecoilAnimation, animationPlayTransition);
        animator.CrossFade(machinegunRecoilAnimation, animationPlayTransition);
    }

    private void Run()
    {
        targetSpeed = runSpeed;
        animator.CrossFade(runAnimation, animationPlayTransition);
    }

    private void Walk()
    {
        targetSpeed = moveSpeed;
        animator.CrossFade(walkAnimation, animationPlayTransition);
    }

    

    public void PlayerTakeDamage(float damage)
    {
        currentPlayerHealth -= damage;
        healthBar.SetHealthBarPercentage(currentPlayerHealth / maxPlayerHealth);
        if (!playerIsDead && currentPlayerHealth <= 0)
        {
            Die();
        }
    }
    private void Die()
    {
        if (currentPlayerHealth <= 0)
        {
            playerIsDead = true;
            ragdoll.ActivateRagdoll();
            Debug.Log("Dead");
            GameManager.Instance.GameOver();
            // Cursor.lockState = CursorLockMode.None;
            
        }
    }
}