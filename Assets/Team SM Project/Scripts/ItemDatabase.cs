using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Code from https://medium.com/@yonem9/create-an-unity-inventory-part-1-basic-data-model-3b54451e25ec

public class ItemDatabase : MonoBehaviour
{
    public List<Item> items = new List<Item>();

    private void Awake()
    {
        BuildDatabase();
    }

    public Item GetItem(int id)
    {
        return items.Find(item => item.id == id);
    }

    public Item GetItem(string itemName)
    {
        return items.Find(item => item.type == itemName);
    }

    void BuildDatabase()
    {
        items = new List<Item>()
        {
            // Dog
            new Item(0, "Dog", "Unnamed Dog",
            new Dictionary<string, int>
            {
                {"Power", 10 },
                {"Speed", 10 }
            }),
            // Cat
            new Item(1, "Cat", "Unnamed Cat",
            new Dictionary<string, int>
            {
                {"Test", 10 }
            }),
            // Wolf
            new Item(2, "Wolf", "Unnamed Wolf",
            new Dictionary<string, int>
            {
                {"Test", 10 }
            }),
            // Eagle
            new Item(3, "Eagle", "Unnamed Eagle",
            new Dictionary<string, int>
            {
                {"Test", 10 }
            }),
            // Cow
            new Item(4, "Cow", "Unnamed Cow",
            new Dictionary<string, int>
            {
                {"Test", 10 }
            }),
            // Pig
            new Item(5, "Pig", "Unnamed Pig",
            new Dictionary<string, int>
            {
                {"Test", 10 }
            }),
            // Horse
            new Item(6, "Horse", "Unnamed Horse",
            new Dictionary<string, int>
            {
                {"Test", 10 }
            }),
            // Sheep
            new Item(7, "Sheep", "Unnamed Sheep",
            new Dictionary<string, int>
            {
                {"Test", 10 }
            })
        };
    }
}
