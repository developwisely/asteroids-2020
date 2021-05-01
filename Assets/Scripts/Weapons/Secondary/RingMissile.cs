using Arsenal;
using UnityEngine;

public class RingMissile : Projectile
{
    private void Awake()
    {
        damage = 100;
        fireRate = 10f;
        scale = new Vector3(4f, 4f, 4f);
        speed = 150;
    }

    // make adjustments for power/speed
}
