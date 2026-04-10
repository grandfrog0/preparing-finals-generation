using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CraftingManager : MonoBehaviour
{
    [SerializeField] CraftingConfig _config;
    [SerializeField] InventoryManager _inventoryManager;

    private void Awake()
    {
        foreach (CraftReceipt receipt in _config.AvailableReceipts)
        {
            Debug.Log((receipt.Result.Name, TryCraft(receipt)));
        }
    }

    public bool TryCraft(CraftReceipt receipt)
    {
        // check if inventory contains needed items
        if (receipt.Elements.Any(x => !Contains(x)))
        {
            return false;
        }

        // remove needed items from inventory
        foreach (ItemElement el in receipt.Elements)
        {
            _inventoryManager.Remove(el.Name, el.Count);
        }

        // so now you can add result item to inventory
        _inventoryManager.Add(receipt.Result.Name, receipt.Result.Count);

        return true;
    }

    public bool Contains(ItemElement el)
    {
        return _inventoryManager.GetCount(el.Name) >= el.Count;
    }
}
