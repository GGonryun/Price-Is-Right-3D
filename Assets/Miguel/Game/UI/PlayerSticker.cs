using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
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
    private Color colorBase = Color.white;
    [Tooltip("")]
    [SerializeField]
    private Color colorSafe = Color.white;
    [Tooltip("")]
    [SerializeField]
    private Color colorWarning = Color.white;
    [Tooltip("")]
    [SerializeField]
    private Color colorDanger = Color.white;
    [Tooltip("")]
    [SerializeField]
    private Color colorCritical = Color.white;
    [Tooltip("")]
    [SerializeField]
    private Color colorSevere = Color.white;

    [Header("Settings")]
    [Tooltip("Pixel offset from the player target")]
    public Vector3 ScreenOffset = new Vector3(0f, 30f, 0f);

    public void Link(Hero hero)
    {
        this.hero = hero;
        PlayerNameText.text = hero.Name;
        characterHeight = hero.Height;
        targetTransform = hero.transform;
    }

    #region UNITY CALLBACKS
    private void Awake()
    {
        transform.SetParent(GameObject.Find("Canvas").transform);
    }

    private void Update()
    {

        if (hero == null)
            Destroy(this.gameObject);
        PlayerStatus.color = SelectColor((int)hero.Multiplier);
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
            case 1:
                return colorBase;
            case 2:
                return colorSafe;
            case 3:
                return colorWarning;
            case 4:
                return colorDanger;
            case 5:
                return colorCritical;
            default:
                return colorSevere;
        }
    }


    private Hero hero;
    private float characterHeight = 0f;
    private Transform targetTransform;
    #endregion
}
