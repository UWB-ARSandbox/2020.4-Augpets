using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public int id;
    public string type;
    public string name;
    public string movement;
    public string owner;
    public bool placed;
    public Sprite icon;
    public Dictionary<string, int> stats = new Dictionary<string, int>();

    // Constructor
    public Item(int id, string type, string movement, string name, string owner, Dictionary<string, int> stats)
    {
        this.id = id;
        this.type = type;
        this.movement = movement;
        this.name = name;
        this.owner = owner;
        this.placed = false;
        this.icon = Resources.Load<Sprite>("Sprites/Items/" + type);
        this.stats = stats;
    }

    // Copy constructor
    public Item(Item item)
    {
        this.id = item.id;
        this.type = item.type;
        this.movement = item.movement;
        this.name = item.name;
        this.owner = item.owner;
        this.placed = item.placed;
        this.icon = Resources.Load<Sprite>("Sprites/Items/" + item.type);
        this.stats = item.stats;
    }

    public void EditName(string newName)
    {
        this.name = newName;
    }
}