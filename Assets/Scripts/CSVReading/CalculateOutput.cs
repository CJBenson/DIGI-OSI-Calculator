using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CalculateOutput : MonoBehaviour
{
    [SerializeField] TMP_Dropdown keyword, category;
    [SerializeField] CSVReader csvReader;
    private TMP_Text outputText;

    void Start()
    {
        outputText = GetComponent<TMP_Text>();
    }

    public void Calculate()
    {
        CSVReader.GroupArray allGroups = csvReader.GetGroupArray();
        string finalOutput = "";

        bool firstMatch = keyword.value == 0;
        bool secondMatch = category.value == 0;

        //Debug.Log("keyword value: " + keyword.options[keyword.value].text + " category value: " + category.options[category.value].text);

        int count = 0;
        foreach (CSVReader.CampusGroup group in allGroups.groupList)
        {
            if(!firstMatch)
                firstMatch = group.keywords.Contains(keyword.options[keyword.value].text);
            if (!secondMatch)
                secondMatch = group.category.Contains(category.options[category.value].text);

            if (firstMatch && secondMatch) 
            {
                finalOutput += group.name + '\n';
                count++;
            }

            firstMatch = keyword.value == 0;
            secondMatch = category.value == 0;
        }
        Debug.Log(count);

        outputText.text = finalOutput;
    }
}
