using Arsenal;
using UnityEngine;

public class LaserBoltMissile : Projectile
{
    private void Awake()
    {
        damage = 1;
        fireRate = 0.325f;
        scale = new Vector3(2, 2, 2);
        speed = 150;
    }
}
