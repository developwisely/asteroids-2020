using Arsenal;
using UnityEngine;

public class LightningBall : Projectile
{
    private void Awake()
    {
        damage = 10;
        fireRate = 5f;
        scale = new Vector3(7f, 7f, 7f);
        speed = 50;
    }

    // Add no destroy on collision, damage over time
}
