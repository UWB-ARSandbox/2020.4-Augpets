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
            // Display name of selected object
            string type = gameObject.name.Remove(gameObject.name.IndexOf("(Clone)"), 7);
            Item temp = inventory.CheckForItem(type);
            // Only save as item if this item is owned by the user
            if(temp.owner == ASL.GameLiftManager.GetInstance().m_Username)
            {
                itemRef = temp;
            }
        }
    } 

    public Item GetItem()
    {
        return itemRef;
    }
}
