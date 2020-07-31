using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arsenal
{
    public class Projectile : MonoBehaviour
    {
        // Settings
        public int damage;
        public float fireRate;
        public Vector3 scale;
        public float speed;

        // Particle prefabs
        public GameObject impactParticle;
        public GameObject projectileParticle;
        public GameObject muzzleParticle;
        public GameObject[] trailParticles;
        [HideInInspector]
        public Vector3 impactNormal;

        private bool hasCollided = false;

        private void Start()
        {
            projectileParticle = Instantiate(projectileParticle, transform.position, transform.rotation) as GameObject;
            projectileParticle.transform.localScale = scale;
            projectileParticle.transform.parent = transform;
            if (muzzleParticle)
            {
                muzzleParticle = Instantiate(muzzleParticle, transform.position, transform.rotation) as GameObject;
                muzzleParticle.transform.rotation = transform.rotation * Quaternion.Euler(180, 0, 0);
                Destroy(muzzleParticle, 1.5f); // Lifetime of muzzle effect.
            }
        }

        public void AddForce(float relativeSpeed = 0)
        {
            GetComponent<Rigidbody>().velocity = transform.forward * (speed + relativeSpeed);
        }

        private void OnCollisionEnter(Collision hit)
        {
            if (hit.gameObject.tag == "Main Camera" || hit.gameObject.tag == "Player") return;

            if (!hasCollided)
            {
                hasCollided = true;
                impactParticle = Instantiate(impactParticle, transform.position, Quaternion.FromToRotation(Vector3.up, impactNormal));

                if (hit.gameObject.tag == "Destructible")
                {
                    Destroy(hit.gameObject);
                }

                foreach (GameObject trail in trailParticles)
                {
                    GameObject curTrail = transform.Find(projectileParticle.name + "/" + trail.name).gameObject;
                    curTrail.transform.parent = null;
                    Destroy(curTrail);
                }
                Destroy(projectileParticle, 3f);
                Destroy(impactParticle, 5f);
                Destroy(gameObject);

                ParticleSystem[] trails = GetComponentsInChildren<ParticleSystem>();
                for (int i = 1; i < trails.Length; i++)
                {
                    ParticleSystem trail = trails[i];
                    if (trail.gameObject.name.Contains("Trail"))
                    {
                        trail.transform.SetParent(null);
                        Destroy(trail.gameObject, 2f);
                    }
                }
            }
        }
    }
}
