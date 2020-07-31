using Arsenal;
using UnityEngine;

public class SphereMissile : Projectile
{
    private void Awake()
    {
        damage = 2;
        fireRate = 0.3f;
        scale = new Vector3(2.5f, 2.5f, 2.5f);
        speed = 160;
    }
}
