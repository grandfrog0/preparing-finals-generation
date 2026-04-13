using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class InventoryManager : MonoBehaviour
{
    public UnityEvent<string, int> OnItemCountChanged = new();

    [SerializeField] InventoryConfig _inventoryConfig;
    private List<ItemElement> Items => _inventoryConfig.Items;

    private void Awake()
    {
        OnItemCountChanged.AddListener((n, c) => Debug.Log((n, c)));
    }

    public int GetCount(string itemName)
    {
        return GetElement(itemName)?.Count ?? 0;
    }

    public void Add(string itemName, int count)
    {
        if (count <= 0)
        {
            return;
        }

        ItemElement element = GetElement(itemName);
        if (element != null)
        {
            element.Count += count;
            OnItemCountChanged.Invoke(element.Name, element.Count);
        }
        else
        {
            Items.Add(new ItemElement(itemName, count));
            OnItemCountChanged.Invoke(itemName, count);
        }

    }

    public void Remove(string itemName, int count)
    {
        if (count <= 0)
        {
            return;
        }

        ItemElement element = GetElement(itemName);
        if (element != null)
        {
            element.Count -= count;
            OnItemCountChanged.Invoke(element.Name, element.Count);
        }
        else
        {
            Debug.LogError($"Inventory does not contains {itemName}!");
        }
    }

    public void Remove(string itemName)
    {
        Items.Remove(GetElement(itemName));
        OnItemCountChanged.Invoke(itemName, 0);
    }

    private ItemElement GetElement(string itemName)
    {
        return Items.FirstOrDefault(x => x.Name == itemName);
    }
}
