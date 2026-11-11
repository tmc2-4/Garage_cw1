using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PhysicsController : MonoBehaviour
{
    //PSEUDO CODE: Declare private variable of type RigidBody, public float speed;
    private Rigidbody rb;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize RigidBody variable by getting from current gameobject;
        rb = GetComponent<Rigidbody>();
    }
    // Fixed Update is called once per frame - use fixed Update for physics
    void FixedUpdate()
    {
        // Check for "a", "d", "w", and "d" key input, for each, add force vector3s going left, right, forward, and back
        if (Input.GetKey("a"))
        {
            rb.AddForce(Vector3.left);
        }
        if (Input.GetKey("d"))
        {
            rb.AddForce(Vector3.right);
        }
        if (Input.GetKey("w"))
        {
            rb.AddForce(Vector3.forward);
        }
        if (Input.GetKey("s")) { 
            rb.AddForce(Vector3.back);

        }
    }
}
//Stretch Tasks
// Unity Rollaball (Combines forces in Vector https://learn.unity.com/project/roll-a-ball-tutorial )
// Intermediate controllers https://medium.com/ironequal/unity-character-controller-vs-rigidbody-a1e243591483