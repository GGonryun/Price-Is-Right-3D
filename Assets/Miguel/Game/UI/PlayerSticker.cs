using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSticker : MonoBehaviour
{
    [Header("Player UI Elements")]
    [Tooltip("")]
    [SerializeField]
    private Text PlayerNameText = null;
    [Tooltip("")]
    [SerializeField]
    private Image PlayerStatus = null;

    [Header("Color Elements")]
    [Tooltip("")]
    [SerializeField]
    private Color colorBase;
    [Tooltip("")]
    [SerializeField]
    private Color colorSafe;
    [Tooltip("")]
    [SerializeField]
    private Color colorWarning;
    [Tooltip("")]
    [SerializeField]
    private Color colorDanger;
    [Tooltip("")]
    [SerializeField]
    private Color colorCritical;

    [Header("Settings")]
    [Tooltip("Pixel offset from the player target")]
    public Vector3 ScreenOffset = new Vector3(0f, 30f, 0f);

    public void Link(Mage mage)
    {
        this.mage = mage;
        PlayerNameText.text = mage.Name;
        characterHeight = mage.Height;
        targetTransform = mage.transform;
    }

    #region UNITY CALLBACKS
    private void Awake()
    {
        transform.SetParent(GameObject.Find("Canvas").transform);
    }
    private void Update()
    {
        if (mage == null)
            Destroy(this.gameObject);
        PlayerStatus.color = SelectColor(Mathf.FloorToInt(mage.Multiplier));
    }

    private void LateUpdate()
    {
        Vector3 position = targetTransform.position;
        position.y += characterHeight;
        this.transform.position = Camera.main.WorldToScreenPoint(position) + ScreenOffset;
    }
    #endregion UNITY CALLBACKS

    #region PRIVATES

    private Color SelectColor(int i)
    {
        if (i < 0) i = 0;

        switch(i)
        {
            case 0:
                return colorBase;
            case 1:
            case 2:
                return colorSafe;
            case 3:
                return colorWarning;
            case 4:
                return colorDanger;
            default:
                return colorCritical;
        }
    }
    private Mage mage;
    private float characterHeight = 0f;
    private Transform targetTransform;
    #endregion
}