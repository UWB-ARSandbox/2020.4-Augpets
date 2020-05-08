using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Code from https://medium.com/@yonem9/create-an-unity-inventory-part-4-display-items-in-ui-6cdac8f734b7

public class UIInventory : MonoBehaviour
{
    public List<UIItem> UIItems = new List<UIItem>();
    public GameObject slotPrefab;
    public Transform slotPanel;
    public int numberOfSlots = 32;

    private void Awake()
    {
        for(int i = 0; i < numberOfSlots; i++)
        {
            GameObject instance = Instantiate(slotPrefab);
            instance.transform.SetParent(slotPanel);
            instance.transform.localScale = new Vector3(1, 1, 1);
            UIItems.Add(instance.GetComponentInChildren<UIItem>());
        }
    }

    public void UpdateSlot(int slot, Item item)
    {
        UIItems[slot].UpdateItem(item);
    }

    public void AddNewItem(Item item)
    {
        UpdateSlot(UIItems.FindIndex(i => i.item == null), item);
    }

    public void RemoveItem(Item item)
    {
        UpdateSlot(UIItems.FindIndex(i => i.item == item), null);
    }

    public void PlaceItem(Item item)
    {
        UpdateSlot(UIItems.FindIndex(i => i.item == item), item);
    }

    public void PickupItem(Item item)
    {
        UpdateSlot(UIItems.FindIndex(i => i.item == item), item);
    }

    public List<UIItem> GetUIItemList()
    {
        return UIItems;
    }
}
