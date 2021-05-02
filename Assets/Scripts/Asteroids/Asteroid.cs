using UnityEngine;

namespace Asteroids
{
    public class Asteroid : MonoBehaviour
    {
        public AsteroidTypes type;
        public float minSpeed;
        public float maxSpeed;
        public float minTumble;
        public float maxTumble;
        public int health;
        public int pointValue;
        public GameObject breakParticle;

        private float _spawnInvulnerability;
        private Rigidbody _rb;
        private MeshCollider _meshCollider;
        private AsteroidManager _asteroidManager;

        // Start is called before the first frame update
        private void Start()
        {
            _rb = GetComponent<Rigidbody>();
            _meshCollider = GetComponent<MeshCollider>();
            _asteroidManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<AsteroidManager>();
            Activate();
        }

        private void Update()
        {
            // Check if spawn cooldown is over, and turn on collider
            if (_spawnInvulnerability > 0)
            {
                _spawnInvulnerability -= Time.deltaTime;
            }
            else
            {
                _spawnInvulnerability = 0;
                _meshCollider.enabled = true;
            }

            // Check for max speed
            if (_rb.velocity.z > maxSpeed)
            {
                _rb.velocity = transform.forward * maxSpeed;
            }
        }

        private void Activate()
        {
            // Set invulnerability spawn timer and turn off collider
            _spawnInvulnerability = 1f;
            _meshCollider.enabled = false;

            // Look within 40 degrees of center stage
            transform.LookAt(new Vector3(0, 0, 0));
            transform.Rotate(transform.rotation.x, (transform.rotation.y + Random.Range(-40.0f, 40.0f)), transform.rotation.z);

            // Move the asteroid
            AddForce();

            // Start tumbling
            Tumble();
        }

        // Applies a random force between minSpeed and maxSpeed
        private void AddForce()
        {
            _rb.velocity = transform.forward * Random.Range(minSpeed, maxSpeed);

            // FEATURE: should velocity min/max change based on level as well?
        }

        // Applies a random tumble between minTumble and maxTumble
        private void Tumble()
        {
            _rb.angularVelocity = Random.insideUnitSphere * Random.Range(minTumble, maxTumble);
        }

        // Applies damage to the asteroid
        public void TakeDamage(int amount)
        {
            health -= amount;
            CheckAsteroid();
        }

        private void CheckAsteroid()
        {
            if (health > 0) return;

            breakParticle = Instantiate(breakParticle, transform.position, transform.rotation);

            switch (type)
            {
                case AsteroidTypes.Small:
                    breakParticle.transform.localScale = new Vector3(2f, 2f, 2f);
                    break;

                case AsteroidTypes.Medium:
                    breakParticle.transform.localScale = new Vector3(3f, 3f, 3f);
                    break;

                case AsteroidTypes.Large:
                    breakParticle.transform.localScale = new Vector3(4f, 4f, 4f);
                    break;
            }

            // Destroy the asteroid
            gameObject.GetComponent<Collider>().enabled = false; // Prevents spawn check from false flag
            _asteroidManager.HandleDestroyedAsteroid(type, transform.position, pointValue);
            Destroy(gameObject);

            // Remove the break particle effect after 5 seconds
            Destroy(breakParticle, 5f);
        }

        private void OnCollisionEnter(Collision hit)
        {
            switch (hit.gameObject.tag)
            {
                case "Projectile":
                    int damage = hit.gameObject.GetComponent<Arsenal.Projectile>().damage;
                    TakeDamage(damage);
                    break;

                case "Player":
                    break;

                case "Asteroid":
                    break;

                default:
                    break;
            }
        }
    }
}