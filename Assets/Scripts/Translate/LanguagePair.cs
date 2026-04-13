using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LanguagePair
{
    public string Key;
    public string Value;

    public LanguagePair(string key, string value)
    {
        Key = key;
        Value = value;
    }
}
