using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class GunSelector : MonoBehaviour
{
    public List<GunSO> guns;
    public Transform gunParent, armRigParent;
    public PlayerController playerController;

    private RigBuilder rigBuilder;
    private GameObject gunModel, armRig;

    private void Start()
    {
        rigBuilder = playerController.GetComponent<RigBuilder>();
    }

    //set active fields rigs, hand job, Pistol, Shotgun, Machine gun
    public void SwitchGun(string gunName)
    {
        for (int i = 0; i < guns.Count; i++)
        {
            if (gunName == guns[i].name)
            {
                //GameObject.Spawn(transforn position)

                for (int j = 0; j < 2; j++) // rigBuilder.layers[i].Count??
                {
                    if (j == i)
                    {
                        rigBuilder.layers[i].active = true;
                    }
                    else
                    {
                        rigBuilder.layers[i].active = false;
                    }
                }

                Spawn(i);
            }
        }
    }

    private void Spawn(int i)
    {
        gunModel = Instantiate(guns[i].gunPrefab);
        gunModel.transform.SetParent(gunParent, false);
        armRig = Instantiate(guns[i].armRigPrefab);
        armRig.transform.SetParent(armRigParent, false);
    }
}