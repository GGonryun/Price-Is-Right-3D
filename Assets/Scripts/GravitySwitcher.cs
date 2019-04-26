using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GravitySwitcher : MonoBehaviour
{
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.GetComponent<Rigidbody>();
    }

    public void ApplyGravity()
    {
        rb.useGravity = true;
    }
}
