using Arsenal;
using UnityEngine;

public class PlasmaMissile : Projectile
{
    private void Awake()
    {
        damage = 3;
        fireRate = 0.25f;
        scale = new Vector3(3f, 3f, 3f);
        speed = 175;
    }
}
