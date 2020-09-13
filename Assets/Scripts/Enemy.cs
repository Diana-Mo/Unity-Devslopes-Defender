using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private Transform exitPoint;
    [SerializeField]
    private GameObject[] waypoints;
    [SerializeField]
    private float navigationUpdate;

    private int target = 0;
    private Transform enemy;
    private float navigationTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        target = 0;
        enemy = GetComponent<Transform>();
        waypoints = GameObject.FindGameObjectsWithTag("checkPoint");
    }

    // Update is called once per frame
    void Update()
    {
        if (waypoints != null)
        {
            navigationTime += Time.deltaTime;
            if (navigationTime > navigationUpdate)
            {
                if (target < waypoints.Length)
                {
                    enemy.position = Vector2.MoveTowards(enemy.position, waypoints[target].transform.position, navigationTime);
                }
                else
                {
                    enemy.position = Vector2.MoveTowards(enemy.position, exitPoint.position, navigationTime);
                }
                navigationTime = 0;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "checkPoint")
            target += 1;
        else if (other.tag == "Finish")
        {
            GameManager.Instance.RemoveEnemyFromScreen();
            Destroy(gameObject);
        }
    }
}
