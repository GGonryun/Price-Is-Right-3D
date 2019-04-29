using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GravitySwitcher : MonoBehaviour
{
    Rigidbody rb;
    void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        rb.isKinematic = false;
    }

    public void ApplyGravity()
    {
        rb.useGravity = true; 
    }

    public void DeactivateGravity()
    {
        rb.useGravity = false;
    }
}
