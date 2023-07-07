using UnityEngine;

namespace ShopSystem
{

[System.Serializable]
public class InventoryItem
{
    [SerializeField] private string Name;
    [SerializeField] private int Price;
    [SerializeField] private Sprite Image;
    [SerializeField][TextArea(3,10)] private string Description;

    [HideInInspector] public GameObject UIObject;

    [ReadOnlyInspector] public int Quantity = 1;
    
    private int IndexInLoadout = 0;

    public InventoryItem() { }
    public InventoryItem(InventoryItem item) 
    { 
        Name = (string)item.Name.Clone();
        Price = item.Price;
        Image = item.Image;

        Description = (string)item.Description.Clone();
        Quantity = item.Quantity;
        UIObject = item.UIObject;
    }

    public string GetName() { return Name; }
    public int GetPrice() { return Price; } 
    public Sprite GetSprite() { return Image; }
    public string GetDescription() { return Description; }
    public int GetQuantity() { return Quantity; }

    public int GetIndexInLoadout() { return IndexInLoadout; }

    public void SetIndexInLoadout(int aindex) { IndexInLoadout = aindex; }
    public void ResetQuantity() { Quantity = 0; } 
    public void ChangeQuantity(int aAmount) { Quantity=Mathf.Max(0,Quantity+aAmount); }
}
}
