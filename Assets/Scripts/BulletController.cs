using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] private GameObject bulletDecal;
    private float speed = 50f;
    private float timeToDestroy = 3f;
    public Vector3 Target { get; set; }
    public bool Hit { get; set; }

    void OnEnable()
    {
        Destroy(gameObject, timeToDestroy);
    }


    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, Target, speed * Time.deltaTime);
        //if shooting in the air, destroy bullet anyway
        if (!Hit && Vector3.Distance(transform.position, Target) < 0.01f) //0.01f is near to target
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        ContactPoint contact = collision.GetContact(0); //get the first contact point
        //instantiate decal in hit point and rotate towards arrival point
        // Instantiate(bulletDecal, contact.point + contact.normal * 0.0001f, Quaternion.LookRotation(contact.normal));
        Destroy(gameObject);
    }
}