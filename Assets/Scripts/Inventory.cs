using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    List<InventoryItem> items = new List<InventoryItem>();
    
    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null) {
            instance = this;
        }
    }

    // Add an item to the inventory
    public void Add(InventoryItem item) {
        items.Add(item);
        Debug.Log("added " + item.name + " to the inventory");
    }

    // Remove an item from the inventory
    public void Remove(InventoryItem item) {
        items.Remove(item);
    }

    // Consume an item in the inventory
    public bool Use(string name) {
        InventoryItem item = null;

        foreach (InventoryItem i in items) {
            if (i.name == name) {
                item = i;
                break;
            }
        }

        if (item == null) {
            return false;
        }
    
        Remove(item);
        return true;
    }
}
