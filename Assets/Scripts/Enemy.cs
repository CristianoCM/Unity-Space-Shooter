using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Configurable Variables
    [Header("Attributes")]
    [SerializeField] float health;
    [SerializeField] int scoreValue = 50;
    [SerializeField] Sprite[] movementSprites;

    [Header("Shot")]
    [SerializeField] float minTimeBetweenShots = 0.2f;
    [SerializeField] float maxTimeBetweenShots = 1f;
    [SerializeField] float timeBetweenShots = 0.5f;
    [SerializeField] bool useRandomTimedShots = false;
    [SerializeField] GameObject laserPrefab;
    [SerializeField] float laserSpeed = 20f;
    [SerializeField] int maxShoots = 5;
    [SerializeField] bool shootsFollowPlayer = true;

    [Header("VFX")]
    [SerializeField] GameObject explosionVFX;
    [SerializeField] float durationExplosionVFX = 1f;

    [Header("SFX")]
    [SerializeField] AudioClip explosionSFX;
    [SerializeField] [Range(0, 1)] float explosionSFXVolume = 0.25f;

    // State Variables
    bool canFire;
    float shotCounter;

    // Cached Variables
    Player player;

    public Sprite[] GetMovementSprites()
    {
        return movementSprites;
    }

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
        canFire = true;
        UpdateShotCounter();
    }

    private void UpdateShotCounter()
    {
        shotCounter = useRandomTimedShots ? Random.Range(minTimeBetweenShots, maxTimeBetweenShots) : timeBetweenShots;
    }

    // Update is called once per frame
    void Update()
    {
        CountDownAndShoot();
    }

    private void CountDownAndShoot()
    {
        if (canFire)
        {
            shotCounter -= Time.deltaTime;
            if (shotCounter <= 0f)
            {
                Fire();
            }
        }
    }

    private void Fire()
    {
        GameObject laser = Instantiate(laserPrefab, transform.position, Quaternion.identity) as GameObject;

        if (shootsFollowPlayer && player != null)
        {
            laser.GetComponent<Rigidbody2D>().velocity = (player.transform.position - transform.position).normalized * laserSpeed;
        }
        else
        {
            laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, laserSpeed * -1);
        }

        CountShoots();
        UpdateShotCounter();
    }

    private void CountShoots()
    {
        maxShoots--;
        if (maxShoots <= 0)
        {
            canFire = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D whatHitMe)
    {
        DamageDealer damageDealer = whatHitMe.gameObject.GetComponent<DamageDealer>();
        if (damageDealer)
        {
            ProcessDamage(damageDealer);
        }
    }

    private void ProcessDamage(DamageDealer damageDealer)
    {
        if (damageDealer != null)
        {
            health -= damageDealer.GetDamage();
            damageDealer.Hit();
        }

        if (health <= 0)
        {
            TriggerExplosionVFX();
            TriggerExplosionSFX();
            FindObjectOfType<GameSession>().AddScore(scoreValue);
            Destroy(gameObject);
        }
    }

    private void TriggerExplosionVFX()
    {
        GameObject explosion = Instantiate(explosionVFX, transform.position, transform.rotation);
        Destroy(explosion, durationExplosionVFX);
    }

    private void TriggerExplosionSFX()
    {
        AudioSource.PlayClipAtPoint(explosionSFX, Camera.main.transform.position, explosionSFXVolume);
    }
}
