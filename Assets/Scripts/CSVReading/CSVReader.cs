using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CSVReader : MonoBehaviour
{
    [SerializeField] private TextAsset textAssetData;
    [SerializeField] private GetCategoryValues categoryAssigner;
    [SerializeField] private GetKeywordValues keywordAssigner;

    [SerializeField] private int elementCount; //set to the number of values in CampusGroup struct
    private int lineCount; //this is NOT user-set, will be filled out by the code


    //Tailored to contain all of the spreadsheet information
    [System.Serializable]
    public struct CampusGroup
    {
        public string name;
        public string type;
        public string category;
        public string mission;
        public string keywords;
        public string link;
    }

    //All the groups available
    [System.Serializable]
    public struct GroupArray
    {
        public CampusGroup[] groupList;
    }

    [SerializeField] private GroupArray allGroups = new GroupArray();


    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("Starting File Read");
        ReadCSV();
        //groupList.SetTextValues(elementCount, lineCount, allGroups);
        keywordAssigner.CollectKeywords(allGroups, lineCount - 1);
        categoryAssigner.CollectCategories(allGroups, lineCount - 1);
    }

    void ReadCSV()
    {
        string csvAsString = textAssetData.text;
        lineCount = 0;

        //loop through string and count number of line breaks
        int charIndex = 0;
        int elementIndex = 0;
        foreach (char c in csvAsString)
        {
            if (c == '~' && csvAsString[charIndex + 1] == '`')
            {
                elementIndex++;
            }
            if (elementIndex == elementCount && c == '\n')
            {
                elementIndex = 0;
                lineCount++;
            }

            charIndex++;
        }

        Debug.Assert(lineCount > 0, "No usable rows of data found in .csv file. The first row is assumed to be data labels");

        //now get number of values in a line
        int numElements = 0;
        char currentCharacter = csvAsString[0];

        if (currentCharacter != '\n')
            numElements = 1;

        charIndex = 0;
        while (currentCharacter != '\n')
        {
            currentCharacter = csvAsString[charIndex];
            if (currentCharacter == ',' && csvAsString[charIndex + 1] != ' ') numElements++;

            charIndex++;
        }

        //only proceed if the number of elements found matches the given number
        Debug.Assert(numElements == elementCount, "Number of elements does not match predicted value");

        //Debug.Log("lineCount: " + lineCount);
        string[] data = csvAsString.Split(new string[] { "~`"}, StringSplitOptions.RemoveEmptyEntries);


        //There are a lot of characters that can be at the start of these strings because the split is messy
        for(int i = 0; i < data.Length; i++)
        {
            data[i] = data[i].TrimStart(',', '"', '\n', '\r', ' ');
            data[i] = data[i].TrimEnd(',', '"', '\n', '\r', ' ');
            //Debug.Log(data[i]);
        }

        allGroups.groupList = new CampusGroup[lineCount - 1];

        for (int j = 0; j < lineCount - 1; j++)
        {
            allGroups.groupList[j] = new CampusGroup();

            int startingElement = elementCount * (j + 1);

            allGroups.groupList[j].name = data[startingElement];
            allGroups.groupList[j].type = data[startingElement + 1];
            allGroups.groupList[j].category = data[startingElement + 2];
            allGroups.groupList[j].mission = data[startingElement + 3];
            allGroups.groupList[j].keywords = data[startingElement + 4];
            allGroups.groupList[j].link = data[startingElement + 5];
        }
    }

    public GroupArray GetGroupArray()
    {
        return allGroups;
    }
}
