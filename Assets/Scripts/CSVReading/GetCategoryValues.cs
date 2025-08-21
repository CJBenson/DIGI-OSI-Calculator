using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetCategoryValues : MonoBehaviour
{
    [SerializeField] MultipleChoiceQuestion menu;
    public void CollectCategories(CSVReader.GroupArray allGroups, int numGroups)
    {
        List<string> allCategories = new List<string>();

        string[] singleGroupKeywords;
        for (int i = 0; i < numGroups; i++)
        {
            singleGroupKeywords = allGroups.groupList[i].category.Split(", ", System.StringSplitOptions.RemoveEmptyEntries);

            foreach (string keyword in singleGroupKeywords)
            {
                string trimmedKeyword = keyword.TrimEnd();
                trimmedKeyword = trimmedKeyword.TrimStart();

                if (!allCategories.Contains(trimmedKeyword))
                    allCategories.Add(trimmedKeyword);
                //else
                    //Debug.Log("Repeat Category: " + trimmedKeyword);
            }
        }

        allCategories.Sort();
        string[] categoriesArray = allCategories.ToArray();

        menu.SetChoicesText(categoriesArray);
    }
}
