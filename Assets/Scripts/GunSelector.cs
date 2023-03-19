using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class GunSelector : MonoBehaviour
{
    public List<GunSO> guns;
    public List<GameObject> armRigs;
    public List<GameObject> weaponsList;
    public List<Transform> barrelsList;
    public Transform gunParent, armRigParent;
    private PlayerController playerController;
    // private Gun gun;

    private RigBuilder rigBuilder;
    private GameObject gunModel, armRig;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        rigBuilder = playerController.GetComponent<RigBuilder>();
    }

    public void SwitchGun(string gunName)
    {
        // Debug.Log("switch gun = " + gunName);
        for (int i = 0; i < guns.Count; i++)
        {
            
            if (gunName == guns[i].name)
            {
                rigBuilder.layers[i].active = true;
                armRigs[i].SetActive(true);
                weaponsList[i].SetActive(true);
                // gun.barrelTransform = barrelsList[i];
            }
            else
            {
                rigBuilder.layers[i].active = false;
                armRigs[i].SetActive(false);
                weaponsList[i].SetActive(false);
            }
        }
    }

    // private void Spawn(int i)
    // {
    //     gunModel = Instantiate(guns[i].gunPrefab);
    //     gunModel.transform.SetParent(gunParent, false);
    //     armRig = Instantiate(guns[i].armRigPrefab);
    //     armRig.transform.SetParent(armRigParent, false);
    // }
}