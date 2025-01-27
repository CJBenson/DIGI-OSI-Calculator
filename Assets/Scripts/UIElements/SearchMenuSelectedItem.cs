using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SearchMenuSelectedItem : MonoBehaviour
{
    [SerializeField] private TMP_Text labelText;
    private string keywordLabel;

    public void Initialize(string keyword)
    {
        keywordLabel = keyword;
        labelText.text = keyword;
    }
    public string GetKeyword() 
    { 
        return keywordLabel;
    }
}
