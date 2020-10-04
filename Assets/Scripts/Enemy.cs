using UnityEngine;

[RequireComponent(typeof(Transform))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Animator))]
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
        //GetComponent allows to address the properties of other  (Les. 38, 5:50)
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
            navigationTime += Time.deltaTime; //keep track on navigationTime since created
            if (navigationTime > navigationUpdate) //not calling things more often than needed
            {
                if (target < waypoints.Length) //not stepping outside of the wayPoints array
                    //move enemies' position
                    enemy.position = Vector2.MoveTowards(enemy.position, waypoints[target].transform.position, navigationTime);
                else
                {
                    //moveTowards exit point
                    enemy.position = Vector2.MoveTowards(enemy.position, exitPoint.transform.position, navigationTime);
                }
                navigationTime = 0;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other) //if we hit something, this will be triggered
    {
        if (other.tag == "checkPoint") //other is the collider 
            target += 1; //new path is aimed at next checkpoint 
        else if (other.tag == "Finish") 
        {
            GameManager.Instance.RoundEscaped += 1;
            GameManager.Instance.TotalEscaped += 1;
            GameManager.Instance.UnregisterEnemy(this);
            GameManager.Instance.isWaveOver(); // check status of the game
        }
        else if (other.tag == "projectile")
        {
            Projectile p = other.gameObject.GetComponent<Projectile>();
            if (!p.isCollided)
            {
                enemyHit(p.AttackStrength);
                p.isCollided = true;
            }

            //Destroy(other.gameObject);
        }
    }

    public void enemyHit(int hitpoints)
    {
        healthPoints -= hitpoints;

        if (healthPoints > 0)
        {
            GameManager.Instance.AudioSource.PlayOneShot(SoundManager.Instance.Hit);
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
        GameManager.Instance.TotalKilled += 1;
        GameManager.Instance.AudioSource.PlayOneShot(SoundManager.Instance.Death);
        GameManager.Instance.addMoney(rewardAmt);
        GameManager.Instance.isWaveOver();
    }
}
