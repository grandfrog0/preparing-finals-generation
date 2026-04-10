using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ItemElement
{
    public string Name;
    public int Count = 1;

    public ItemElement(string name, int count)
    {
        Name = name;
        Count = count;
    }
}
