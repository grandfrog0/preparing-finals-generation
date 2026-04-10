using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "crafting", menuName = "SO/Crafting/CraftingConfig")]
public class CraftingConfig : ScriptableObject
{
    public List<CraftReceipt> AvailableReceipts;
}
