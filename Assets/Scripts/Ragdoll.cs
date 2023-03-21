using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Ragdoll : MonoBehaviour
{
    private Rigidbody[] rigidBodies;
    private Animator animator;
    void Start()
    {
        rigidBodies = GetComponentsInChildren<Rigidbody>();
        animator = GetComponent<Animator>();
        
        DeactivateRagdoll();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DeactivateRagdoll()
    {
        foreach (var rigidBody in rigidBodies)
        {
            rigidBody.isKinematic = true;
        }

        animator.enabled = true;
    }
    public void ActivateRagdoll()
    {
        foreach (var rigidBody in rigidBodies)
        {
            rigidBody.isKinematic = false;
        }
        animator.enabled = false;
    }

    public void ApplyForce(Vector3 force)
    {
        var rigidBody = animator.GetBoneTransform(HumanBodyBones.Hips).GetComponent<Rigidbody>();
        rigidBody.AddForce(force, ForceMode.VelocityChange);
    }
}
