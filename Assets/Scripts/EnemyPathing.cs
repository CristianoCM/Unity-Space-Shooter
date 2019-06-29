using System.Collections.Generic;
using UnityEngine;

public class EnemyPathing : MonoBehaviour
{
    // Cached Variables
    WaveConfig waveConfig;
    Enemy enemy;
    SpriteRenderer enemySpriteRenderer;

    // State Variables
    List<Transform> enemyPath;
    int waypointIndex;
    Sprite[] movementSprites;
    bool thereAreMovementSprites;
    bool destroyOnLastWaypoint;

    // Start is called before the first frame update
    void Start()
    {
        enemy = gameObject.GetComponent<Enemy>();
        enemySpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        movementSprites = enemy.GetMovementSprites();
        thereAreMovementSprites = movementSprites.Length > 0;
        enemyPath = waveConfig.GetPathPrefab();
        destroyOnLastWaypoint = waveConfig.GetDestroyOnLastWaypoint();
        waypointIndex = 0;
        transform.position = enemyPath[waypointIndex].position;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    public void SetWaveConfig(WaveConfig waveConfig)
    {
        this.waveConfig = waveConfig;
    }

    private void Move()
    {
        if (waypointIndex < enemyPath.Count)
        {
            var targetPosition = enemyPath[waypointIndex].position;
            var movementThisFrame = waveConfig.GetMoveSpeed() * Time.deltaTime;

            if (thereAreMovementSprites)
            {
                ChangeMovementSprite(transform.position.x, targetPosition.x);
            }

            transform.position = Vector2.MoveTowards(transform.position, targetPosition, movementThisFrame);

            if (targetPosition == transform.position)
            {
                waypointIndex++;
            }
        }
        else
        {
            if (destroyOnLastWaypoint)
            {
                Destroy(gameObject);
            }
        }
    }

    private void ChangeMovementSprite(float currentX, float newPosX)
    {
        if (currentX > newPosX)
        {
            enemySpriteRenderer.sprite = movementSprites[2];
        }
        else if (currentX < newPosX)
        {
            enemySpriteRenderer.sprite = movementSprites[1];
        }
        else
        {
            enemySpriteRenderer.sprite = movementSprites[0];
        }
    }
}
