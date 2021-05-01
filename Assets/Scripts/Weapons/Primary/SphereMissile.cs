using Arsenal;
using UnityEngine;

public class SphereMissile : Projectile
{
    private void Awake()
    {
        damage = 2;
        fireRate = 0.3f;
        scale = new Vector3(3.5f, 3.5f, 3.5f);
        speed = 160;
    }
}
