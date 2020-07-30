using UnityEngine;

namespace Arsenal
{
    public class FirePrimary : MonoBehaviour
    {
        [SerializeField]
        // Array of projectiles to choose from
        public GameObject[] projectiles;

        [Header("Missile spawns at attached game objects")]
        // Left and Right cannon spawn points
        public Transform leftSpawnPosition;
        public Transform rightSpawnPosition;

        [HideInInspector]
        // Currently selected projectile
        public int currentProjectile = 0;
        
        private float _canFireIn;
        private Rigidbody _playerRb;
        private GameObject _selectedProjectile;

        private void Start()
        {
            // TODO: set up which weapon is selected from user choice

            _playerRb = GetComponent<Rigidbody>();
            _selectedProjectile = projectiles[currentProjectile];
        }

        private void Update()
        {
            // Update cooldown
            _canFireIn -= Time.deltaTime;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                HandleProjectileFire();
            }
        }

        private void HandleProjectileFire()
        {
            // Check cooldown before firing
            if (_canFireIn > 0) return;

            GameObject leftProjectile = Instantiate(_selectedProjectile, leftSpawnPosition.position, leftSpawnPosition.rotation) as GameObject;
            GameObject rightProjectile = Instantiate(_selectedProjectile, rightSpawnPosition.position, rightSpawnPosition.rotation) as GameObject;

            if (_playerRb.velocity.magnitude > 0)
            {
                leftProjectile.GetComponent<Projectile>().AddForce(_playerRb.velocity.magnitude);
                rightProjectile.GetComponent<Projectile>().AddForce(_playerRb.velocity.magnitude);
            }
            else
            {
                leftProjectile.GetComponent<Projectile>().AddForce();
                rightProjectile.GetComponent<Projectile>().AddForce();
            }
        }
    }
}
