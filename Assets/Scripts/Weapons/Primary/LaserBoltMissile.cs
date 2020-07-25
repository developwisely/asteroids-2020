using Arsenal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBoltMissile : Projectile
{
    private void Awake()
    {
        fireRate = 10;
        scale = new Vector3(2, 2, 1.25f);
        speed = 150;
    }
}
