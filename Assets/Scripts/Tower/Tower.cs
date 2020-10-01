using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField]
    private float timeBetweenAttacks;
    [SerializeField]
    private float attackRadius;
    [SerializeField]
    private Projectile projectile;
    private Enemy targetEnemy = null;
    private float attackCounter = 0;
    private bool isAttacking = true;

    // Start is called before the first frame update
    void Start()
    {
        attackCounter = 0;
        isAttacking = true;
    }

    // Update is called once per frame
    void Update()
    {
        attackCounter -= Time.deltaTime;
        if (targetEnemy == null || targetEnemy.IsDead) {
            Enemy nearestEnemy = GetNearestEnemyInRange();
            if (nearestEnemy != null && Vector2.Distance(transform.localPosition, nearestEnemy.transform.localPosition) <= attackRadius )
            {
                targetEnemy = nearestEnemy;
            }
        }
        else
        {
            if (Vector2.Distance(transform.localPosition, targetEnemy.transform.localPosition) > attackRadius)
                targetEnemy = null;
        }

        if (attackCounter <= 0)
        {
            isAttacking = true;
        }
    }

    private void FixedUpdate()
    {
        if (isAttacking)
        {
            Attack();
        }
    }

    public void Attack()
    {
        if (targetEnemy == null)
        {
            return;
        }

        attackCounter = timeBetweenAttacks;
        isAttacking = false;

        Projectile newProjectile = Instantiate(projectile);
        newProjectile.transform.localPosition = transform.localPosition;

        //move projectile to enemy
        StartCoroutine(MoveProjectile(newProjectile));
    }
    IEnumerator MoveProjectile(Projectile p)
    {
        //loop until projectile hit the enemy
        while (getTargetDistance(targetEnemy) > 0.20f && targetEnemy != null && !targetEnemy.IsDead && (getTargetDistance(targetEnemy) < attackRadius))
        {
            if (p.isCollided)
            {
                break;
            }

            var dir = targetEnemy.transform.localPosition - transform.localPosition;
            var angleDirection = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            p.transform.rotation = Quaternion.AngleAxis(angleDirection, Vector3.forward);
            p.transform.localPosition = Vector2.MoveTowards(p.transform.localPosition, targetEnemy.transform.localPosition, 5f * Time.deltaTime);
            yield return null;
        }

        targetEnemy = null;
        Destroy(p.gameObject);
    }

    private float getTargetDistance(Enemy thisEnemy)
    {
        if(thisEnemy == null)
        {
            thisEnemy = GetNearestEnemyInRange();
            if (thisEnemy == null)
                return 0f;
        }
        return Mathf.Abs(Vector2.Distance(transform.localPosition, thisEnemy.transform.localPosition));
    }

    private List<Enemy> GetEnemiesInRange()
    {
        List<Enemy> enemiesInRange = new List<Enemy>();
        foreach(Enemy enemy in GameManager.Instance.EnemyList)
        {
            if (Vector2.Distance(transform.localPosition, enemy.transform.localPosition) <= attackRadius)
                enemiesInRange.Add(enemy);
        }
        return enemiesInRange;
    }

    private Enemy GetNearestEnemyInRange()
    {
        Enemy nearestEnemy = null;
        float smallestDistance = float.PositiveInfinity;
        foreach(Enemy enemy in GetEnemiesInRange())
        {
             if (Vector2.Distance(transform.localPosition, enemy.transform.localPosition) < smallestDistance)
            {
                smallestDistance = Vector2.Distance(transform.localPosition, enemy.transform.localPosition);
                nearestEnemy = enemy;
            }
        }
        return nearestEnemy;
    } 
}
