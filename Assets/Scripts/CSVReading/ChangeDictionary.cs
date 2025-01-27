using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeDictionary : MonoBehaviour
{
    [SerializeField] List<string> selectableTerms, correspondingKeywords;

    public string[] ConvertToKeywords(string[] inWords)
    {
        string[] returnArray = inWords;

        for(int i = 0; i < inWords.Length; i++)
        {
            if (selectableTerms.Contains(inWords[i]))
                returnArray[i] = correspondingKeywords[selectableTerms.IndexOf(inWords[i])];
        }

        return returnArray;
    }
}
