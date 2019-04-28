using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour
{
    [Tooltip("Duration")]
    [SerializeField]
    private float delay = 3.0f;

    public Player Owner { get; private set; }

    public static void Link(Spell spell, Player owner)
    {
        spell.Owner = owner;
    }

    #region UNITY CALLBACKS
    public void Start()
    {
        Destroy(gameObject, delay);
    }
    #endregion UNITY CALLBACKS
}
