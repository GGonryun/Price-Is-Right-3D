using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    [Tooltip("")]
    public static Settings Instance;

    public Character Character { get; set; } = Character.Wizard;

    #region UNITY CALLBACKS
    private void Awake()
    {
        Instance = this;
    }
    #endregion UNITY CALLBACKS


}
