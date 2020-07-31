using Arsenal;
using UnityEngine;

public class PlasmaMissile : Projectile
{
    private void Awake()
    {
        damage = 3;
        fireRate = 0.25f;
        scale = new Vector3(2.25f, 2.25f, 2.25f);
        speed = 175;
    }
}
