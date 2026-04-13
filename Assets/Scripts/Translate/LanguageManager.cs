using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.VirtualTexturing.Debugging;

public class LanguageManager : MonoBehaviour
{
    [SerializeField] string language = "en_en";

    private static LanguageBundle _languageBundle;
    public static string GetText(string translateId)
    {
        return _languageBundle.Items.FirstOrDefault(x => x.Key == translateId).Value ?? "";
    }

    private void Start()
    {
        Init(language);
    }

    public void Init(string language)
    {
        LoadBundle(language);
        TranslateTexts();
    }

    private void LoadBundle(string language)
    {
        TextAsset textAsset = (TextAsset)Resources.Load("Language/" + language);
        Debug.Log(textAsset.text);
        _languageBundle = JsonUtility.FromJson<LanguageBundle>(textAsset.text);
    }

    private void TranslateTexts()
    {
        TranslatableText[] texts = FindObjectsByType<TranslatableText>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        foreach (TranslatableText text in texts)
        {
            text.Translate();
            Debug.Log(text);
        }
    }
}
