using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SearchMenuUnselectedItem : MonoBehaviour
{
    [SerializeField] TMP_Text labelText;
    private string keyword;

    public void Initialize(string word)
    {
        keyword = word;
        labelText.text = keyword;
    }
    public string GetKeyword()
    {
        return keyword;
    }
}
