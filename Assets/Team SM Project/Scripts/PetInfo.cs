using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetInfo : MonoBehaviour
{
    public Inventory inventory;
    public Item itemRef;
    private void Awake()
    {
        // Get inventory
        inventory = GameObject.Find("Pet Manager").GetComponent<Inventory>();
    }

    private void Start()
    {
        if(gameObject.name.Contains("(Clone)"))
        {
            // Get reference to item and store in object
            string type = gameObject.name.Remove(gameObject.name.IndexOf("(Clone)"), 7);
            itemRef = inventory.CheckForItem(type);
        }
    } 

    public Item GetItem()
    {
        return itemRef;
    }

    public bool TryGetStat(string statName, out int value)
    {
        return itemRef.stats.TryGetValue(statName, out value);
    }
}
