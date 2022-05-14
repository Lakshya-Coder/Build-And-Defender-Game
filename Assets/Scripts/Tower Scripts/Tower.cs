using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] private float shootTimerMax;
    private float shootTimer;
    private Enemy targetEnemy;
    private float lookForTargetTimer;
    private float lookForTargetTimerMax = 0.2f;
    private Transform projectileSpawnPosition;

    private void Awake() => projectileSpawnPosition = transform.Find("projectileSpawnPosition");

    private void Update()
    {
        HandleTargeting();
        HandleShooting();
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

    private void HandleShooting()
    {
        shootTimer -= Time.deltaTime;

        if (shootTimer <= 0)
        {
            shootTimer = shootTimerMax;
            if (targetEnemy != null)
                ArrowProjectile.Create(projectileSpawnPosition.position, targetEnemy);
        }
    }

    private void LookForTarget()
    {
        float targetMaxRadius = 20f;
        Collider2D[] collider2DArray = Physics2D.OverlapCircleAll(transform.position, targetMaxRadius);

        foreach (Collider2D collider2D in collider2DArray)
        {
            Enemy enemy = collider2D.GetComponent<Enemy>();

            if (enemy != null)
            {
                // Is a enemy!
                if (targetEnemy == null)
                    targetEnemy = enemy;
                else
                {
                    if (Vector3.Distance(transform.position, enemy.transform.position) <
                        Vector3.Distance(transform.position, targetEnemy.transform.position))
                        // Closer!
                        targetEnemy = enemy;
                }
            }
        }
    }
}