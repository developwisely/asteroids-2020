using Arsenal;
using UnityEngine;

public class Shockwave : Projectile
{
    private void Awake()
    {
        damage = 100;
        fireRate = 1f;
        scale = new Vector3(7f, 7f, 7f);
        speed = 100;
    }

    // add damage + gravity force on impact particle
}