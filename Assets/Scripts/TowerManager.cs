using UnityEngine;
using UnityEngine.EventSystems;

public class TowerManager : Singleton<TowerManager>
{

    public TowerBtn towerBtnPressed{get; set;}
    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        //require SpriteRenderer for game to work
        //look at previous game for example
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);

            if (hit.collider.tag == "buildSite")
            {
                //hit.collider.tag = "buildSiteFull";
                placeTower(hit);
            }
        }
        if (spriteRenderer.enabled)
        {
            followMouse();
        }
    }

    public void placeTower(RaycastHit2D hit)
    {
        if(!EventSystem.current.IsPointerOverGameObject() && towerBtnPressed != null)
        {
            GameObject newTower = Instantiate(towerBtnPressed.TowerObject);
            newTower.transform.position = hit.transform.position;
            hit.collider.tag = "buildSiteFull";
            disableDragSprite();
        }
    }

    public void selectedTower(TowerBtn towerSelected)
    {
        towerBtnPressed = towerSelected;
        enableDragSprite(towerBtnPressed.DragSprite);
        //Debug.Log("Pressed! " + towerBtnPressed.gameObject);
    }

    public void followMouse()
    {
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector2(transform.position.x, transform.position.y);
    }

    public void enableDragSprite(Sprite sprite)
    {
        spriteRenderer.enabled = true;
        spriteRenderer.sprite = sprite;
    }

    public void disableDragSprite()
    {
        spriteRenderer.enabled = false;
    }
}
