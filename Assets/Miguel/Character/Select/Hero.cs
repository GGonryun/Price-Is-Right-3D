using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviourPun
{
    [Tooltip("The hero's user interface prefab")]
    [SerializeField]
    private GameObject heroInterfacePrefab = null;
}
