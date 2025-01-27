using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GetKeywordValues : MonoBehaviour
{
    [SerializeField] SearchableMenu menu;
    [SerializeField] private int repeatsForSignificance;
    public void CollectKeywords(CSVReader.GroupArray allGroups, int numGroups)
    {
        List<string> allKeywords = new List<string>();
        List<string> significantKeywords = new List<string>();

        string[] singleGroupKeywords;
        for(int i = 0; i < numGroups; i++) 
        { 
            singleGroupKeywords = allGroups.groupList[i].keywords.Split(", ", System.StringSplitOptions.None);

            foreach(string keyword in singleGroupKeywords) 
            {
                allKeywords.Add(keyword);
            }
        }

        allKeywords.Sort();

        foreach(string keyword in allKeywords)
        {
            if(!significantKeywords.Contains(keyword) 
               && allKeywords.LastIndexOf(keyword) - allKeywords.IndexOf(keyword) + 1 >= repeatsForSignificance)
            {
                significantKeywords.Add(keyword);
            }
        }
        foreach(string keyword in significantKeywords)
        {
            menu.LoadKeyword(keyword);
        }
    }
}
