using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GunSO gun;
    [SerializeField] private GameObject bulletPrefab;
    
    [SerializeField] private Transform bulletParent;
    [SerializeField] private Transform barrelTransform;
    private Transform cameraTransform;
    [SerializeField] private float bulletHitMissDistance = 25f;
    private Animator animator;
    private int recoilAnimation;
    [SerializeField] private float animationPlayTransition = 0.15f;
    // private PlayerController playerController;
    private float timeSinceLastShot;
    private ParticleSystem muzzleFlash;
    private void Awake()
    {
        // playerController = GetComponent<PlayerController>();
        
    }

    private void OnEnable()
    {
        
//        PlayerController.ShootAction.performed += _ => Shoot();
    }

    private void OnDisable()
    {
        PlayerController.ShootAction.performed -= _ => Shoot();
        PlayerController.ReloadAction.performed -= _ => StartReload();
        
    }

    private void Start()
    {
        cameraTransform = Camera.main.transform;
        barrelTransform = transform.Find("Barrel");
        PlayerController.ShootAction.performed += _ => Shoot();
        muzzleFlash = transform.Find("MuzzleFlash").GetComponent<ParticleSystem>();
        PlayerController.ReloadAction.performed += _ => StartReload();

        recoilAnimation = Animator.StringToHash("Pistol Shoot Recoil");
        // animator = playerController.GetComponent<Animator>();
    }

    private void Update()
    {
        timeSinceLastShot += Time.deltaTime;
    }

    private bool CanShoot() => !gun.reloading && timeSinceLastShot > 1f / (gun.fireRate / 60f);
    public void Shoot()
    {
        
        if (gun.currentAmmo > 0)
        {
            if (CanShoot())
            {
                Debug.Log("shoot");
                RaycastHit hit;
                GameObject bullet = Instantiate(bulletPrefab, barrelTransform.position, Quaternion.identity,
                    bulletParent);
                BulletController bulletController = bullet.GetComponent<BulletController>();
                if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, Mathf.Infinity))
                {
                    
                    bulletController.Target = hit.point;
                    bulletController.Hit = true;
                    Debug.Log(hit.transform.name);
                    IDamageable damageable = hit.transform.GetComponent<IDamageable>();
                    damageable?.TakeDamage(gun.damage);
                }
                else
                {
                    bulletController.Target = cameraTransform.position + cameraTransform.forward * bulletHitMissDistance;
                    bulletController.Hit = false;
                }

                gun.currentAmmo--;
                timeSinceLastShot = 0;
                OnGunShot();
            }
            
        }
        else
        {
            Debug.Log("Empty mag!");
        }

        
        

        // animator.CrossFade(recoilAnimation, animationPlayTransition);
    }
    private void OnGunShot()
    {
        //for fx
        muzzleFlash.Emit(1);
    }

    public void StartReload()
    {
        gun.currentAmmo = gun.magSize;
        gun.reloading = false;
        Debug.Log("Reloaded!");
        
    }

   
}
