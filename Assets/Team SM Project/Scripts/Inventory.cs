using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Code from https://medium.com/@yonem9/create-an-unity-inventory-part-2-configure-the-inventory-3a990eff8cba

public class Inventory : MonoBehaviour
{
    public List<Item> playerItems = new List<Item>();
    public ItemDatabase itemDatabase;
    public UIInventory inventoryUI;

    private void Start()
    {
        // Give all Items
        for(int i = 0; i < itemDatabase.GetNumItems(); i++)
        {
            GiveItem(i);
        }
    }

    public void GiveItem(int id)
    {
        Item itemToAdd = itemDatabase.GetItem(id);
        playerItems.Add(itemToAdd);
        inventoryUI.AddNewItem(itemToAdd);
        Debug.Log("Added item: " + itemToAdd.type);
    }

    public void GiveItem(string itemName)
    {
        Item itemToAdd = itemDatabase.GetItem(itemName);
        playerItems.Add(itemToAdd);
        inventoryUI.AddNewItem(itemToAdd);
        Debug.Log("Added item: " + itemToAdd.type);
    }

    public Item CheckForItem(int id)
    {
        return playerItems.Find(item => item.id == id);
    }

    public Item CheckForItem(string itemName)
    {
        return playerItems.Find(item => item.type == itemName);
    }

    public void PlaceItem(int id)
    {
        Item itemToPlace = CheckForItem(id);
        if(itemToPlace != null)
        {
            itemToPlace.placed = true;
            inventoryUI.PlaceItem(itemToPlace);
            Debug.Log("Placed item: " + itemToPlace.type);
        }
    }

    public void PickupItem(int id)
    {
        Item itemToPickup = CheckForItem(id);
        if(itemToPickup != null)
        {
            itemToPickup.placed = false;
            inventoryUI.PickupItem(itemToPickup);
            Debug.Log("Picked up item: " + itemToPickup.type);
        }
    }

    public void RemoveItem(int id)
    {
        Item itemToRemove = CheckForItem(id);
        if(itemToRemove != null)
        {
            playerItems.Remove(itemToRemove);
            inventoryUI.RemoveItem(itemToRemove);
            Debug.Log("Removed item: " + itemToRemove.type);
        }
    }

    public List<Item> GetItemList()
    {
        return playerItems;
    }

    public Item GetItem(int slot)
    {
        return playerItems[slot];
    }
}
