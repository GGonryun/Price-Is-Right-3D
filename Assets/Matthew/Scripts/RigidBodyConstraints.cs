using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidBodyConstraints : MonoBehaviour
{
    Rigidbody rb;
    void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeAll;
    }
    void UnfreezeConstraints()
    {
        rb.constraints = RigidbodyConstraints.None;
    }

}
