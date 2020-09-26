using UnityEngine;

public class TowerBtn : MonoBehaviour
{
   [SerializeField]
   private GameObject towerObject;
   [SerializeField]
   private Sprite dragSprite;
   [SerializeField]
   private int towerPrice;

   public GameObject TowerObject
    {
        get
        {
            return towerObject;
        }
    }

    public Sprite DragSprite
    {
        get
        {
            return dragSprite;
        }
    }

    public int TowerPrice
    {
        get
        {
            return towerPrice;
        }
    }
}
