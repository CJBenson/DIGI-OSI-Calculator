using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GroupList : MonoBehaviour
{
    [SerializeField] private GameObject listElement;
    [SerializeField] private Transform[] headers;

    public void SetTextValues(int elementCount, int lineCount, CSVReader.GroupArray allGroups)
    {
        Debug.Assert(headers.Length == elementCount, "Number of headers does not match .csv file element count");

        //Create list elements for each header
        for (int i = 0; i < elementCount; i++)
        {
            for (int j = 0; j < lineCount - 1; j++)
            {
                GameObject element = Instantiate(listElement, headers[i]);
                TextMeshProUGUI elementText = element.GetComponent<TextMeshProUGUI>();

                //pick which value it has based on what the current element is
                if (i == 0) elementText.text = allGroups.groupList[j].name;
                else if (i == 1) elementText.text = allGroups.groupList[j].type;
                else if (i == 2) elementText.text = allGroups.groupList[j].category;
                else if (i == 3) elementText.text = allGroups.groupList[j].mission;
                else if (i == 4) elementText.text = allGroups.groupList[j].keywords;
            }
        }
    }
}
