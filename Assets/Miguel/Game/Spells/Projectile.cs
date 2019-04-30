using UnityEngine;

public sealed class Projectile : Spell
{
    [Header("Stats")]
    [Tooltip("")]
    [SerializeField]
    private float speed = 1f;

    private void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
    }
}
