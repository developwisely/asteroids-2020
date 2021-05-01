using UnityEngine;

namespace Arsenal
{
    public class FireSecondary : MonoBehaviour
    {
        [SerializeField]
        // Array of projectiles to choose from
        public GameObject[] projectiles;

        [Header("Missile spawns at attached game object")]
        // Center cannon spawn points
        public Transform centerSpawnPosition;

        // Currently selected projectile
        public int currentProjectile = 0;

        private float _canFireIn;
        private Rigidbody _playerRb;
        private GameObject _selectedProjectile;

        void Start()
        {
            // TODO: set up which weapon is selected from user choice

            _playerRb = GetComponent<Rigidbody>();
            _selectedProjectile = projectiles[currentProjectile];
        }

        void Update()
        {
            // Update cooldown
            _canFireIn -= Time.deltaTime;

            if (Input.GetButton("Fire2"))
            {
                HandleProjectileFire();
            }
        }

        private void HandleProjectileFire()
        {
            // Check cooldown before firing
            if (_canFireIn > 0) return;

            GameObject centerProjectile = Instantiate(_selectedProjectile, centerSpawnPosition.position, centerSpawnPosition.rotation) as GameObject;

            if (_playerRb.velocity.magnitude > 0)
            {
                centerProjectile.GetComponent<Projectile>().AddForce(_playerRb.velocity.magnitude);
            }
            else
            {
                centerProjectile.GetComponent<Projectile>().AddForce();
            }

            _canFireIn = centerProjectile.GetComponent<Projectile>().fireRate;
        }
    }
}