using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    public static void Create(Vector3 position)
    {
        Transform enemyTransform = Instantiate(GameAssets.Instance.pfEnemy, position, Quaternion.identity);
        Enemy enemy = enemyTransform.GetComponent<Enemy>();
    }

    private Transform targetTransform;
    private Rigidbody2D rigidbody2d;
    private float lookForTargetTimer;
    private float lookForTargetTimerMax = 0.2f;
    private HealthSystem healthSystem;

    private void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        
        if (BuildingManager.Instance.GetHQBuilding() != null)
            targetTransform = BuildingManager.Instance.GetHQBuilding().transform;
        
        healthSystem = GetComponent<HealthSystem>();
        healthSystem.OnDied += HealthSystem_OnDied; 
        healthSystem.OnDamaged += HealthSystem_OnDamaged; 

        lookForTargetTimer = Random.Range(0f, lookForTargetTimerMax);
    }

    private void HealthSystem_OnDamaged(object sender, EventArgs e)
    {
        SoundManager.Instance.PlaySound(SoundManager.Sound.EnemyHit);
        CinemachineShake.Instance.ShakeCamera(5f, 0.1f);
        ChromaticAberrationEffect.Instance.SetWeight(0.5f);
    }

    private void HealthSystem_OnDied(object sender, EventArgs e)
    {
        Instantiate(GameAssets.Instance.pfEnemyDieParticles, transform.position, Quaternion.identity);
        SoundManager.Instance.PlaySound(SoundManager.Sound.EnemyDie);
        CinemachineShake.Instance.ShakeCamera(7f, 0.15f);
        ChromaticAberrationEffect.Instance.SetWeight(0.5f);
        Destroy(gameObject);
    }

    private void Update()
    {
        HandleMovement();
        HandleTargeting();
    }

    private void HandleTargeting()
    {
        lookForTargetTimer -= Time.deltaTime;
        if (lookForTargetTimer <= 0)
        {
            LookForTarget();
            lookForTargetTimer += lookForTargetTimerMax;
        }
    }

    private void HandleMovement()
    {
        if (targetTransform != null)
        {
            Vector3 moveDir = (targetTransform.position - transform.position).normalized;

            float moveSpeed = 6f;
            rigidbody2d.velocity = moveDir * moveSpeed;
        }
        else
            rigidbody2d.velocity = Vector2.zero;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Building building = collision.gameObject.GetComponent<Building>();

        if (building != null)
        {
            // Collided with a building!
            HealthSystem healthSystem = building.GetComponent<HealthSystem>();
            healthSystem.Damage(10);
            this.healthSystem.Damage(999);
        }
    }

    private void LookForTarget()
    {
        float targetMaxRadius = 10f;
        Collider2D[] collider2DArray = Physics2D.OverlapCircleAll(transform.position, targetMaxRadius);

        foreach (Collider2D collider2D in collider2DArray)
        {
            Building building = collider2D.GetComponent<Building>();

            if (building != null)
            {
                // Is a building!
                if (targetTransform == null)
                    targetTransform = building.transform;
                else
                {
                    if (Vector3.Distance(transform.position, building.transform.position) <
                        Vector3.Distance(transform.position, targetTransform.position))
                        // Closer!
                        targetTransform = building.transform;
                }
            }
        }

        if (targetTransform == null)
            // Found no targets within range!
            if (BuildingManager.Instance.GetHQBuilding() != null)
                targetTransform = BuildingManager.Instance.GetHQBuilding().transform;
    }
}