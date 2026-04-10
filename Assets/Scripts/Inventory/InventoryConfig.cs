using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditorInternal;
using UnityEngine;

[CreateAssetMenu(fileName = "Inventory", menuName = "SO/Inventory/InventoryConfig")]
public class InventoryConfig : ScriptableObject
{
    public List<ItemElement> Items;
}
