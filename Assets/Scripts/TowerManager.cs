using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider2D))]

public class TowerManager : Singleton<TowerManager>
{

    public TowerBtn towerBtnPressed{get; set;}

    private SpriteRenderer spriteRenderer;
    private List<Tower> TowerList = new List<Tower>();
    //renaming build tiles which are colliders
    private List<Collider2D> BuildList = new List<Collider2D>();
    //something to put in that list
    private Collider2D buildTile;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        //allows to reference buildTile colliders, so we can rename them
        buildTile = GetComponent<Collider2D>();
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

            if (hit.collider != null && hit.collider.tag == "buildSite")
            {
                buildTile = hit.collider;
                buildTile.tag = "buildSiteFull"; //why did I have this commented???
                RegisterBuildSite(buildTile);
                placeTower(hit);
            }
        }
        if (spriteRenderer.enabled)
        {
            followMouse();
        }
    }

     public void RegisterBuildSite(Collider2D buildTag)
    {
        BuildList.Add(buildTag);
    }

    //type Tower, variable named tower
    public void RegisterTower(Tower tower)
    {
        TowerList.Add(tower);
    }

    public void RenameTagsBuildSites()
    {
        //rename the hit.collider tag back to build site, so that when the towers are removed we will be able to build a new one on its place
        //go through the list and rename it to build list
        foreach(Collider2D buildTag in BuildList)
        {
            buildTag.tag = "buildSite";
        }
        //make sure the list is then empty
        BuildList.Clear();
    }

    //remove the towers when the game starts over
    public void DestroyAllTower()
    {
        foreach(Tower tower in TowerList)
        {
            Destroy(tower.gameObject);
        }
        TowerList.Clear();
    }

    public void placeTower(RaycastHit2D hit)
    {
        if(!EventSystem.current.IsPointerOverGameObject() && towerBtnPressed != null)
        {
            Tower newTower = Instantiate(towerBtnPressed.TowerObject);
            newTower.transform.position = hit.transform.position;
            hit.collider.tag = "buildSiteFull";
            buyTower(towerBtnPressed.TowerPrice);
            GameManager.Instance.AudioSource.PlayOneShot(SoundManager.Instance.TowerBuilt);
            RegisterTower(newTower);
            disableDragSprite();
        }
    }

    public void buyTower(int price)
    {
        // when try to buy a tower, subtract money
        GameManager.Instance.subtractMoney(price); 
    }

    public void selectedTower(TowerBtn towerSelected)
    {
        //don't select tower if don't have enough money
        if (towerSelected.TowerPrice <= GameManager.Instance.TotalMoney)
        {
            towerBtnPressed = towerSelected;
            enableDragSprite(towerBtnPressed.DragSprite);
            //Debug.Log("Pressed! " + towerBtnPressed.gameObject);
        }
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
