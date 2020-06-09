using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Code from https://medium.com/@yonem9/create-an-unity-inventory-part-1-basic-data-model-3b54451e25ec

public class ItemDatabase : MonoBehaviour
{
    public List<Item> items = new List<Item>();

    private string unnamed = "Unnamed";

    private const string land = "Land";
    private const string aerial = "Aerial";
    private const string aquatic = "Aquatic";

    void Awake()
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
            new Item(0, "Dog", land, unnamed, 
            new Dictionary<string, int>
            {
                {"Food", 10 },
                {"Exercise", 10 },
                {"Affection", 10 },
                {"Speed", 2}
            }),
            // Cat
            new Item(1, "Cat", land, unnamed,
            new Dictionary<string, int>
            {
                {"Food", 10 },
                {"Exercise", 10 },
                {"Affection", 10 },
                {"Speed", 2}
            }),
            // Wolf
            new Item(2, "Wolf", land, unnamed,
            new Dictionary<string, int>
            {
                {"Food", 5 },
                {"Exercise", 7 },
                {"Affection", 3 },
                {"Speed", 2}
            }),
            // Eagle
            new Item(3, "Eagle", aerial, unnamed,
            new Dictionary<string, int>
            {
                {"Food", 10 },
                {"Exercise", 10 },
                {"Affection", 10 },
                {"Speed", 3}
            }),
            // Cow
            new Item(4, "Cow", land, unnamed,
            new Dictionary<string, int>
            {
                {"Food", 10 },
                {"Exercise", 10 },
                {"Affection", 10 },
                {"Speed", 1}
            }),
            // Pig
            new Item(5, "Pig", land, unnamed,
            new Dictionary<string, int>
            {
                {"Food", 1 },
                {"Exercise", 1 },
                {"Affection", 1 },
                {"Speed", 1}
            }),
            // Horse
            new Item(6, "Horse", land, unnamed,
            new Dictionary<string, int>
            {
                {"Food", 10 },
                {"Exercise", 10 },
                {"Affection", 10 },
                {"Speed", 5}
            }),
            // Sheep
            new Item(7, "Sheep", land, unnamed,
            new Dictionary<string, int>
            {
                {"Food", 0 },
                {"Exercise", 0 },
                {"Affection", 0 },
                {"Speed", 2}
            }),
            // Baby Dragon
            new Item(8, "Baby Dragon", aerial, unnamed,
            new Dictionary<string, int>
            {
                {"Food", 10 },
                {"Exercise", 10 },
                {"Affection", 10 },
                {"Speed", 4}
            }),
            // Velociraptor
            new Item(9, "Velociraptor", land, unnamed,
            new Dictionary<string, int>
            {
                {"Food", 10 },
                {"Exercise", 10 },
                {"Affection", 10 },
                {"Speed", 4}
            }),
            // Tyrannosaurus Rex
            new Item(10, "Tyrannosaurus Rex", land, unnamed,
            new Dictionary<string, int>
            {
                {"Food", 10 },
                {"Exercise", 10 },
                {"Affection", 10 },
                {"Speed", 2}
            }),
            // Stegosaurus
            new Item(11, "Stegosaurus", land, unnamed,
            new Dictionary<string, int>
            {
                {"Food", 10 },
                {"Exercise", 10 },
                {"Affection", 10 },
                {"Speed", 1}
            }),
            // Triceratops
            new Item(12, "Triceratops", land, unnamed,
            new Dictionary<string, int>
            {
                {"Food", 10 },
                {"Exercise", 10 },
                {"Affection", 10 },
                {"Speed", 1}
            }),
            // Apatosaurus
            new Item(13, "Apatosaurus", land, unnamed,
            new Dictionary<string, int>
            {
                {"Food", 10 },
                {"Exercise", 10 },
                {"Affection", 10 },
                {"Speed", 1}
            }), 
            // Parasaurolophus
            new Item(14, "Parasaurolophus", land, unnamed,
            new Dictionary<string, int>
            {
                {"Food", 10 },
                {"Exercise", 10 },
                {"Affection", 10 },
                {"Speed", 1}
            })
        };
    }
}
