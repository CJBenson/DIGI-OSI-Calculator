using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CompileAvailableData : MonoBehaviour
{
    [SerializeField] MultipleChoiceQuestion categoryQuestion, genderQuestion, religionQuestion,
                                            cultureYesNo, genderYesNo, religionYesNo;

    [SerializeField] SearchableMenu cultureSearch, keywordSearch;

    [SerializeField] TMP_InputField email;

    [SerializeField] CSVReader reader;
    [SerializeField] GameObject template;

    [SerializeField] EmailSend emailSender;

    List<GameObject> allResults = new List<GameObject>();

    private bool mustMeetAllGuidelines = false;

    private string[] chosenCategories, chosenCultures, chosenGenders, chosenReligions, chosenKeywords;

    public void ToggleMeetAllGuidelines()
    {
        mustMeetAllGuidelines = !mustMeetAllGuidelines;
    }

    //Compile the data into readable arrays
    public void ReadDataToArrays()
    {
        //Read the Yes/No answers
        MultipleChoiceElement[] cultureYN = cultureYesNo.GetAnswerOptions();
        MultipleChoiceElement[] genderYN = genderYesNo.GetAnswerOptions();
        MultipleChoiceElement[] religionYN = religionYesNo.GetAnswerOptions();

        //Read the multiple choice for the elements
        MultipleChoiceElement[] genders;
        MultipleChoiceElement[] religions;
        SearchMenuSelectedItem[] cultures;

        MultipleChoiceElement[] categories = categoryQuestion.GetSelectedOptions();

        if (genderYN[0].GetSelectedState())
            genders = genderQuestion.GetSelectedOptions();
        else
            genders = new MultipleChoiceElement[0];

        if (religionYN[0].GetSelectedState())
            religions = religionQuestion.GetSelectedOptions();
        else
            religions = new MultipleChoiceElement[0];

        //Read the search for the elements
        if (cultureYN[0].GetSelectedState())
            cultures = cultureSearch.GetSelections();
        else
            cultures = new SearchMenuSelectedItem[0];

        SearchMenuSelectedItem[] keywords = keywordSearch.GetSelections();

        //Make the string arrays
        chosenCategories = new string[categories.Length];
        chosenCultures = new string[cultures.Length];
        chosenGenders = new string[genders.Length];
        chosenReligions = new string[religions.Length];
        chosenKeywords = new string[keywords.Length];

        //Assign the values of the string arrays
        for(int i = 0; i < categories.Length; i++)
        {
            MultipleChoiceElement categoryChoice = categories[i];

            chosenCategories[i] = categoryChoice.GetText();
        }
        for (int i = 0; i < cultures.Length; i++)
        {
            SearchMenuSelectedItem cultureChoice = cultures[i];

            chosenCultures[i] = cultureChoice.GetKeyword();
        }
        for (int i = 0; i < genders.Length; i++)
        {
            MultipleChoiceElement genderChoice = genders[i];

            chosenGenders[i] = genderChoice.GetText();
        }
        for (int i = 0; i < religions.Length; i++)
        {
            MultipleChoiceElement religionChoice = religions[i];

            chosenReligions[i] = religionChoice.GetText();
        }
        for (int i = 0; i < keywords.Length; i++)
        {
            SearchMenuSelectedItem keywordChoice = keywords[i];

            chosenKeywords[i] = keywordChoice.GetKeyword();
        }

        //Use the dictionary to convert question choice values into usable keywords
        ChangeDictionary dictionary = reader.gameObject.GetComponent<ChangeDictionary>();
        chosenCultures = dictionary.ConvertToKeywords(chosenCultures);
        chosenGenders = dictionary.ConvertToKeywords(chosenGenders);
        chosenReligions = dictionary.ConvertToKeywords(chosenReligions);
    }

    public CSVReader.CampusGroup[] ReturnGroups()
    {
        //Debug.Log("Starting Return");
        CSVReader.GroupArray groupArray = reader.GetGroupArray();

        List<CSVReader.CampusGroup> groupResults = new List<CSVReader.CampusGroup>();
        List<int> criteria = new List<int>();

        CSVReader.CampusGroup[] finalResults = new CSVReader.CampusGroup[5];

        bool firstCriterion = false;
        bool secondCriterion = false;
        bool thirdCriterion = false;
        bool fourthCriterion = false;
        bool fifthCriterion = false;

        foreach (CSVReader.CampusGroup campusGroup in groupArray.groupList)
        {
            if (!mustMeetAllGuidelines)
            {
                foreach (string indivCategory in chosenCategories)
                {
                    //Debug.Log(indivCategory + " is category, group is: " + campusGroup.name + " and categories list is: " + campusGroup.category);
                    if (campusGroup.category.Contains(indivCategory, System.StringComparison.CurrentCultureIgnoreCase) &&
                        !groupResults.Contains(campusGroup))
                    {
                        groupResults.Add(campusGroup);
                        criteria.Add(1);

                        //Debug.Log(campusGroup.name + " contained a category match!");
                    }
                    else if (campusGroup.category.Contains(indivCategory, System.StringComparison.CurrentCultureIgnoreCase) && 
                             groupResults.Contains(campusGroup))
                    {
                        criteria[groupResults.IndexOf(campusGroup)]++;

                        //Debug.Log(campusGroup.name + " contained a category match!");
                    }
                }
                foreach (string indivCulture in chosenCultures)
                {
                    if (campusGroup.keywords.Contains(indivCulture, System.StringComparison.CurrentCultureIgnoreCase) &&
                        !groupResults.Contains(campusGroup))
                    {
                        groupResults.Add(campusGroup);
                        criteria.Add(2);

                        //Debug.Log(campusGroup.name + " contains " + indivCulture + ": " + campusGroup.keywords);
                    }
                    else if (campusGroup.category.Contains("Cultural") &&
                             campusGroup.keywords.Contains(indivCulture, System.StringComparison.CurrentCultureIgnoreCase) && 
                             groupResults.Contains(campusGroup))
                    {
                        criteria[groupResults.IndexOf(campusGroup)] += 2;
                        //Debug.Log(campusGroup.name + " contains " + indivCulture + ": " + campusGroup.keywords);
                    }
                    else
                    {
                        //Debug.Log(campusGroup.name + " does not contain " + indivCulture + ": " + campusGroup.keywords);
                    }
                }
                foreach (string indivGender in chosenGenders)
                {
                    if (campusGroup.keywords.Contains(indivGender, System.StringComparison.CurrentCultureIgnoreCase) &&
                        !groupResults.Contains(campusGroup))
                    {
                        groupResults.Add(campusGroup);
                        criteria.Add(2);

                        //Debug.Log(campusGroup.name + " contained a gender match!");
                    }
                    else if (campusGroup.keywords.Contains(indivGender, System.StringComparison.CurrentCultureIgnoreCase) && 
                             groupResults.Contains(campusGroup))
                    {
                        criteria[groupResults.IndexOf(campusGroup)] += 2;

                        //Debug.Log(campusGroup.name + " contained a gender match!");
                    }
                }
                foreach (string indivReligion in chosenReligions)
                {
                    if (campusGroup.category.Contains("Religious and Spiritual") &&
                        campusGroup.keywords.Contains(indivReligion, System.StringComparison.CurrentCultureIgnoreCase) &&
                        !groupResults.Contains(campusGroup))
                    {
                        groupResults.Add(campusGroup);
                        criteria.Add(2);

                        //Debug.Log(campusGroup.name + " contained a religion match!");
                    }
                    else if (campusGroup.category.Contains("Religious and Spiritual") &&
                             campusGroup.keywords.Contains(indivReligion, System.StringComparison.CurrentCultureIgnoreCase) && 
                             groupResults.Contains(campusGroup))
                    {
                        criteria[groupResults.IndexOf(campusGroup)] += 2;

                        //Debug.Log(campusGroup.name + " contained a religion match!");
                    }
                }
                foreach (string indivKeyword in chosenKeywords)
                {
                    if (campusGroup.keywords.Contains(indivKeyword, System.StringComparison.CurrentCultureIgnoreCase) &&
                        !groupResults.Contains(campusGroup))
                    {
                        groupResults.Add(campusGroup);
                        criteria.Add(3);

                        //Debug.Log(campusGroup.name + " contained a keyword match!");
                        //Debug.Log(indivKeyword + " is keyword, group is: " + campusGroup.name + " and keywords list is: " + campusGroup.keywords);
                    }
                    else if (campusGroup.keywords.Contains(indivKeyword, System.StringComparison.CurrentCultureIgnoreCase) && 
                             groupResults.Contains(campusGroup))
                    {
                        criteria[groupResults.IndexOf(campusGroup)] += 3;

                        //Debug.Log(campusGroup.name + " contained a keyword match!");
                        //Debug.Log(indivKeyword + " is keyword, group is: " + campusGroup.name + " and keywords list is: " + campusGroup.keywords);
                    }
                }

                if (chosenCultures.Length <= 0)
                {
                    if (campusGroup.category.Contains("Cultural") &&
                        groupResults.Contains(campusGroup))
                    {
                        criteria.RemoveAt(groupResults.IndexOf(campusGroup));
                        groupResults.Remove(campusGroup);

                        //Debug.Log(campusGroup.name + "removed for cultural reason");
                    }
                }
                if (chosenGenders.Length <= 0)
                {
                    if ((campusGroup.keywords.Contains("men", System.StringComparison.CurrentCultureIgnoreCase) ||
                        campusGroup.keywords.Contains("women", System.StringComparison.CurrentCultureIgnoreCase) ||
                        campusGroup.keywords.Contains("non-binary", System.StringComparison.CurrentCultureIgnoreCase) ||
                        campusGroup.keywords.Contains("fraternity", System.StringComparison.CurrentCultureIgnoreCase) ||
                        campusGroup.keywords.Contains("sorority", System.StringComparison.CurrentCultureIgnoreCase))
                        &&
                        groupResults.Contains(campusGroup))
                    {
                        criteria.RemoveAt(groupResults.IndexOf(campusGroup));
                        groupResults.Remove(campusGroup);

                        //Debug.Log(campusGroup.name + "removed for gender reason");
                    }
                }
                if (chosenReligions.Length <= 0)
                {
                    if (campusGroup.category.Contains("Religious and Spiritual") &&
                        groupResults.Contains(campusGroup))
                    {
                        criteria.RemoveAt(groupResults.IndexOf(campusGroup));
                        groupResults.Remove(campusGroup);

                        //Debug.Log(campusGroup.name + "removed for religious reason");
                    }
                }
            }
            else
            {
                if (chosenCultures.Length <= 0) secondCriterion = true;
                if (chosenGenders.Length <= 0) thirdCriterion = true;
                if (chosenReligions.Length <= 0) fourthCriterion = true;

                foreach (string indivCategory in chosenCategories)
                {
                    if (campusGroup.category.Contains(indivCategory, System.StringComparison.CurrentCultureIgnoreCase))
                        firstCriterion = true;
                }
                foreach (string indivCulture in chosenCultures)
                {
                    if (campusGroup.category.Contains("Cultural") &&
                        campusGroup.keywords.Contains(indivCulture, System.StringComparison.CurrentCultureIgnoreCase))
                        secondCriterion = true;
                }
                foreach (string indivGender in chosenGenders)
                {
                    if (campusGroup.keywords.Contains(indivGender, System.StringComparison.CurrentCultureIgnoreCase))
                        thirdCriterion = true;
                }
                foreach (string indivReligion in chosenReligions)
                {
                    if (campusGroup.category.Contains("Cultural") &&
                        campusGroup.keywords.Contains(indivReligion, System.StringComparison.CurrentCultureIgnoreCase))
                        fourthCriterion = true;
                }
                foreach (string indivKeyword in chosenKeywords)
                {
                    if (campusGroup.keywords.Contains(indivKeyword, System.StringComparison.CurrentCultureIgnoreCase))
                        fifthCriterion = true;
                }

                if (firstCriterion && secondCriterion && thirdCriterion && fourthCriterion && fifthCriterion)
                {
                    groupResults.Add(campusGroup);
                    criteria.Add(5);
                }


                firstCriterion = false;
                secondCriterion = false;
                thirdCriterion = false;
                fourthCriterion = false;
                fifthCriterion = false;
            }
        }

        Debug.Assert(groupResults.Count == criteria.Count, "COUNTS DO NOT MATCH");

        for (int i = 0; i < 5; i++)
        {
            //You must have at least one match
            if (criteria.Count > 0)
            {
                //Set the max to the first element
                int maxCriteria = criteria[0];
                int indexOfMax = 0;

                for (int ind = 0; ind < criteria.Count; ind++)
                {
                    //Find the number of matches for the current item
                    int currentInt = criteria[ind];

                    if (currentInt > maxCriteria)
                    {
                        maxCriteria = currentInt;
                        indexOfMax = ind;
                    }
                }
                //Debug.Log("final result " + i + ": " + groupResults[indexOfMax].name);
                //Debug.Log("group had a criteria count of: " + criteria[indexOfMax]);

                //Set the next item in the array to that group
                finalResults[i] = groupResults[indexOfMax];
                criteria.RemoveAt(indexOfMax);
                groupResults.RemoveAt(indexOfMax);
            }
        }

        emailSender.SendEmailWithData(email.text, finalResults);

        return finalResults;
    }

    public void UpdateText()
    {
        ReadDataToArrays();
        CSVReader.CampusGroup[] returnedNames = ReturnGroups();

        foreach(GameObject go in allResults) 
        {
            Destroy(go);
        }

        allResults.Clear();
        
        for (int i = 0; i < returnedNames.Length; i++)
        {
            GameObject linkGO = Instantiate(template, transform);
            Link link = linkGO.GetComponent<Link>();
            link.SetLink(returnedNames[i].link);
            
            TMP_Text linkText = linkGO.GetComponent<TMP_Text>();
            linkText.text = returnedNames[i].name;

            allResults.Add(linkGO);
        }

        foreach (GameObject go in allResults)
        {
            go.SetActive(true);
        }
    }
}
