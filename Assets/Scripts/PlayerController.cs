using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController), typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    public float targetSpeed;
    [SerializeField] private float moveSpeed = 2.0f;
    [SerializeField] private float runSpeed = 5.0f;
    [SerializeField] private float jumpHeight = 1.0f;
    [SerializeField] private float gravityValue = -9.81f;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform barrelTransform;
    [SerializeField] private Transform bulletParent;
    [SerializeField] private float bulletHitMissDistance = 25f;
    [SerializeField] private float animationSmoothTime = 0.1f;
    [SerializeField] private float animationPlayTransition = 0.15f;
    [SerializeField] private Transform aimTarget;
    [SerializeField] private float aimDistance = 10f;

    private CharacterController controller;
    private PlayerInput playerInput;
    private Vector3 playerVelocity;
    private bool groundedPlayer;

    private InputAction moveAction, jumpAction, shootAction, runAction;
    
    [SerializeField] private InputActionReference actionReference;

    private Transform cameraTransform;

    private Animator animator;
    private int moveXAnimationParameterId, moveZAnimationParameterId;
    private Vector2 currentAnimationBlendVector;
    private Vector2 animationVelocity;
    private int jumpAnimation, recoilAnimation, runAnimation, walkAnimation;
    

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();

        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];
        shootAction = playerInput.actions["Shoot"];
        runAction = playerInput.actions["Run"];

        cameraTransform = Camera.main.transform;
        Cursor.lockState = CursorLockMode.Locked;
        
        //Animations
        animator = GetComponent<Animator>();
        moveXAnimationParameterId = Animator.StringToHash("MoveX");
        moveZAnimationParameterId = Animator.StringToHash("MoveZ");
        jumpAnimation = Animator.StringToHash("Pistol Jump");
        recoilAnimation = Animator.StringToHash("Pistol Shoot Recoil");
        runAnimation = Animator.StringToHash("Run");
        walkAnimation = Animator.StringToHash("Walk");
    }

    private void Start()
    {
        runAction.started += _ =>
        {
            targetSpeed = runSpeed;
            animator.CrossFade(runAnimation, animationPlayTransition);
        };
        runAction.canceled += _ =>
        {
            targetSpeed = moveSpeed; 
            animator.CrossFade(walkAnimation, animationPlayTransition);
        };
    }

    private void OnEnable()
    {
        shootAction.performed += _ => ShootGun();
        
        // actionReference.action.Enable();
    }

    private void OnDisable()
    {
        shootAction.performed -= _ => ShootGun();
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
    }

    private void ShootGun()
    {
        RaycastHit hit;
        GameObject bullet = GameObject.Instantiate(bulletPrefab, barrelTransform.position, Quaternion.identity,
            bulletParent);
        BulletController bulletController = bullet.GetComponent<BulletController>();
        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, Mathf.Infinity))
        {
            bulletController.Target = hit.point;
            bulletController.Hit = true;
        }
        else
        {
            bulletController.Target = cameraTransform.position + cameraTransform.forward * bulletHitMissDistance;
            bulletController.Hit = false;
        }
        animator.CrossFade(recoilAnimation, animationPlayTransition);
    }
    private void Run()
    {
        targetSpeed = runSpeed;
    }
}