using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TranslatableText : MonoBehaviour
{
    [SerializeField] string _translateId;
    private Text _text;

    public void Translate()
    {
        if (_text == null)
        {
            _text = GetComponent<Text>();
        }
        _text.text = LanguageManager.GetText(_translateId);
        Debug.Log(_text.text);
    }
}
