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
            new Item(0, "Dog", "Unnamed",
            new Dictionary<string, int>
            {
                {"Power", 10 },
                {"Speed", 10 },
                {"Flight", 0}
            }),
            // Cat
            new Item(1, "Cat", "Unnamed",
            new Dictionary<string, int>
            {
                {"Power", 10 },
                {"Speed", 10 },
                {"Flight", 0}
            }),
            // Wolf
            new Item(2, "Wolf", "Unnamed",
            new Dictionary<string, int>
            {
                {"Power", 10 },
                {"Speed", 10 },
                {"Flight", 0}
            }),
            // Eagle
            new Item(3, "Eagle", "Unnamed",
            new Dictionary<string, int>
            {
                {"Power", 10 },
                {"Speed", 10 },
                {"Flight", 1}
            }),
            // Cow
            new Item(4, "Cow", "Unnamed",
            new Dictionary<string, int>
            {
                {"Power", 10 },
                {"Speed", 10 },
                {"Flight", 0}
            }),
            // Pig
            new Item(5, "Pig", "Unnamed",
            new Dictionary<string, int>
            {
                {"Power", 10 },
                {"Speed", 10 },
                {"Flight", 0}
            }),
            // Horse
            new Item(6, "Horse", "Unnamed",
            new Dictionary<string, int>
            {
                {"Power", 10 },
                {"Speed", 10 },
                {"Flight", 0}
            }),
            // Sheep
            new Item(7, "Sheep", "Unnamed",
            new Dictionary<string, int>
            {
                {"Power", 10 },
                {"Speed", 10 },
                {"Flight", 0}
            }),
            // Baby Dragon
            new Item(8, "Baby Dragon", "Unnamed",
            new Dictionary<string, int>
            {
                {"Power", 10 },
                {"Speed", 10 },
                {"Flight", 1}
            })
        };
    }
}
