using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private Transform exitPoint;
    //[SerializeField]
    private GameObject[] waypoints;
    [SerializeField]
    private float navigationUpdate;
    [SerializeField]
    private int healthPoints;
    [SerializeField]
    private int rewardAmt;

    private int target = 0;
    private Transform enemy;
    private Collider2D enemyCollider;
    private Animator anim;
    private float navigationTime = 0;
    private bool isDead = false;

    public bool IsDead
    {
        get
        {
            return isDead;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //target = 0;
        enemy = GetComponent<Transform>();
        enemyCollider = GetComponent<Collider2D>();
        waypoints = GameObject.FindGameObjectsWithTag("checkPoint");
        anim = GetComponent<Animator>();

        GameManager.Instance.RegisterEnemy(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (waypoints != null && !isDead)
        {
            navigationTime += Time.deltaTime;
            if (navigationTime > navigationUpdate)
            {
                if (target < waypoints.Length)
                    enemy.position = Vector2.MoveTowards(enemy.position, waypoints[target].transform.position, navigationTime);
                else
                {
                    enemy.position = Vector2.MoveTowards(enemy.position, exitPoint.position, navigationTime);
                }
                navigationTime = 0;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "checkPoint")
            target += 1;
        else if (other.tag == "Finish")
        {
            GameManager.Instance.UnregisterEnemy(this);
        }
        else if (other.tag == "projectile")
        {
            Projectile newP = other.gameObject.GetComponent<Projectile>();
            enemyHit(newP.AttackStrength);
            Destroy(other.gameObject);
        }
    }

    public void enemyHit(int hitpoints)
    {
        if (healthPoints - hitpoints > 0)
        {
            healthPoints -= hitpoints;
            anim.Play("Hurt");
        }
        else
        {
            anim.SetTrigger("didDie");
            die();
        }
    }

    public void die()
    {
        isDead = true;
        enemyCollider.enabled = false;
    }
}
