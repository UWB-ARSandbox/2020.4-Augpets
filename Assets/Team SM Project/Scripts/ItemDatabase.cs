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

    public int GetNumItems()
    {
        return items.Count;
    }

    void BuildDatabase()
    {
        items = new List<Item>()
        {
            // Dog
            new Item(0, "Dog", "Ground", "Unnamed",
            new Dictionary<string, int>
            {
                {"Food", 10 },
                {"Exercise", 10 },
                {"Affection", 10 }
            }),
            // Cat
            new Item(1, "Cat", "Ground", "Unnamed",
            new Dictionary<string, int>
            {
                {"Food", 10 },
                {"Exercise", 10 },
                {"Affection", 10 }
            }),
            // Wolf
            new Item(2, "Wolf", "Ground", "Unnamed",
            new Dictionary<string, int>
            {
                {"Food", 5 },
                {"Exercise", 7 },
                {"Affection", 3 }
            }),
            // Eagle
            new Item(3, "Eagle", "Flight", "Unnamed",
            new Dictionary<string, int>
            {
                {"Food", 10 },
                {"Exercise", 10 },
                {"Affection", 10 }
            }),
            // Cow
            new Item(4, "Cow", "Ground", "Unnamed",
            new Dictionary<string, int>
            {
                {"Food", 10 },
                {"Exercise", 10 },
                {"Affection", 10 }
            }),
            // Pig
            new Item(5, "Pig", "Ground", "Unnamed",
            new Dictionary<string, int>
            {
                {"Food", 1 },
                {"Exercise", 1 },
                {"Affection", 1 }
            }),
            // Horse
            new Item(6, "Horse", "Ground", "Unnamed",
            new Dictionary<string, int>
            {
                {"Food", 10 },
                {"Exercise", 10 },
                {"Affection", 10 }
            }),
            // Sheep
            new Item(7, "Sheep", "Ground", "Unnamed",
            new Dictionary<string, int>
            {
                {"Food", 0 },
                {"Exercise", 0 },
                {"Affection", 0 }
            }),
            // Baby Dragon
            new Item(8, "Baby Dragon", "Flight", "Unnamed",
            new Dictionary<string, int>
            {
                {"Food", 10 },
                {"Exercise", 10 },
                {"Affection", 10 }
            }),
            // Velociraptor
            new Item(9, "Velociraptor", "Ground", "Unnamed",
            new Dictionary<string, int>
            {
                {"Food", 10 },
                {"Exercise", 10 },
                {"Affection", 10 }
            }),
            // Tyrannosaurus Rex
            new Item(10, "Tyrannosaurus Rex", "Ground", "Unnamed",
            new Dictionary<string, int>
            {
                {"Food", 10 },
                {"Exercise", 10 },
                {"Affection", 10 }
            }),
            // Stegosaurus
            new Item(11, "Stegosaurus", "Ground", "Unnamed",
            new Dictionary<string, int>
            {
                {"Food", 10 },
                {"Exercise", 10 },
                {"Affection", 10 }
            }),
            // Triceratops
            new Item(12, "Triceratops", "Ground", "Unnamed",
            new Dictionary<string, int>
            {
                {"Food", 10 },
                {"Exercise", 10 },
                {"Affection", 10 }
            }),
            // Apatosaurus
            new Item(13, "Apatosaurus", "Ground", "Unnamed",
            new Dictionary<string, int>
            {
                {"Food", 10 },
                {"Exercise", 10 },
                {"Affection", 10 }
            }), 
            // Parasaurolophus
            new Item(14, "Parasaurolophus", "Ground", "Unnamed",
            new Dictionary<string, int>
            {
                {"Food", 10 },
                {"Exercise", 10 },
                {"Affection", 10 }
            })
        };
    }
}
