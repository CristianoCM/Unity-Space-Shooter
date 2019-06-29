using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Configurable Variables
    [Header("Player")]
    [SerializeField] float moveSpeed = 7f;
    [SerializeField] float health = 200;
    [SerializeField] float maxHealth = 200;
    [SerializeField] float damageBase = 50;
    [SerializeField] Sprite[] movementSprites;

    [Header("Laser")]
    [SerializeField] GameObject laserPrefab;
    [SerializeField] float laserSpeed = 20f;
    [SerializeField] float fireDelay = 0.1f;

    [Header("VFX")]
    [SerializeField] GameObject explosionVFX;
    [SerializeField] float durationExplosionVFX = 1f;

    [Header("SFX")]
    [SerializeField] AudioClip explosionSFX;
    [SerializeField] [Range(0, 1)] float explosionSFXVolume = 0.25f;
    [SerializeField] AudioClip laserSFX;
    [SerializeField] [Range(0, 1)] float laserSFXVolume = 0.25f;
    [SerializeField] AudioClip beingHitSFX;
    [SerializeField] [Range(0, 1)] float beingHitSFXVolume = 0.25f;

    // State Variables
    float cameraXMin;
    float cameraXMax;
    float cameraYMin;
    float cameraYMax;
    Coroutine fireContinuously;
    bool canFire;

    // Cached Variables
    SpriteRenderer spriteRenderer;
    Level level;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        level = FindObjectOfType<Level>();
        SetUpMoveBoundaries();
        canFire = true;
    }

    private void SetUpMoveBoundaries()
    {
        Camera gameCamera = Camera.main;

        cameraXMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + (spriteRenderer.bounds.size.x / 2);
        cameraXMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - (spriteRenderer.bounds.size.x / 2);

        cameraYMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + (spriteRenderer.bounds.size.y / 2);
        cameraYMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - (spriteRenderer.bounds.size.y / 2);
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Fire();
    }

    private void Fire()
    {
        if (Input.GetButton("Fire1") && canFire)
        {
            fireContinuously = StartCoroutine(FireContinuously());
        }
    }

    private IEnumerator FireContinuously()
    {
        GameObject laser = Instantiate(laserPrefab, transform.position, Quaternion.identity) as GameObject;
        laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, laserSpeed);
        TriggerShotSFX();
        canFire = false;
        yield return new WaitForSeconds(fireDelay);
        canFire = true;
    }

    private void Move()
    {
        var deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
        var deltaY = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;

        var newPosX = Mathf.Clamp(transform.position.x + deltaX, cameraXMin, cameraXMax);
        var newPosY = Mathf.Clamp(transform.position.y + deltaY, cameraYMin, cameraYMax);

        ChangeMovementSprite(transform.position.x, newPosX);

        transform.position = new Vector2(newPosX, newPosY);
    }

    private void ChangeMovementSprite(float currentX, float newPosX)
    {
        if (currentX > newPosX)
        {
            spriteRenderer.sprite = movementSprites[1];
        }
        else if (currentX < newPosX)
        {
            spriteRenderer.sprite = movementSprites[2];
        }
        else
        {
            spriteRenderer.sprite = movementSprites[0];
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
            TriggerHitSFX();
        }

        if (health <= 0)
        {
            FindObjectOfType<DisplayHealth>().SetZeroHealth();
            TriggerExplosionVFX();
            TriggerExplosionSFX();
            Destroy(gameObject);
            level.LoadGameOverScreen();
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

    private void TriggerShotSFX()
    {
        AudioSource.PlayClipAtPoint(laserSFX, Camera.main.transform.position, laserSFXVolume);
    }

    private void TriggerHitSFX()
    {
        AudioSource.PlayClipAtPoint(beingHitSFX, Camera.main.transform.position, beingHitSFXVolume);
    }

    public float GetHealth()
    {
        return health;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }

    public float GetDamageBase()
    {
        return damageBase;
    }
}
