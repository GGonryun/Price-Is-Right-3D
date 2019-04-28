using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour
{
    public Player Owner { get; private set; }

    public void Start()
    {
        Destroy(gameObject, 3.0f);
    }

    public void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }

    public void Initialize(Player owner)
    {
        Owner = owner;
    }
}
