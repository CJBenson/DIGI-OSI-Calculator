using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MultipleChoiceQuestion: MonoBehaviour
{
    [SerializeField] int maxAnswers;
    [SerializeField] Sprite circleBubble, squareBubble, circleFilled, squareFilled;
    [SerializeField] MultipleChoiceElement templateChoice;
    [SerializeField] string[] choices;
    MultipleChoiceElement[] toggles;

    // Start is called before the first frame update
    private void Start()
    {
        Initialize();
    }
    private void Initialize()
    {
        toggles = new MultipleChoiceElement[choices.Length];

        for(int i = 0; i < choices.Length; i++)
        {
            GameObject newChoiceGO = Instantiate(templateChoice.gameObject, transform);
            MultipleChoiceElement newChoice = newChoiceGO.GetComponent<MultipleChoiceElement>();
            newChoice.SetText(choices[i]);
            toggles[i] = newChoice;

            newChoiceGO.SetActive(true);
        }

        if (maxAnswers == 1)
        {
            foreach (MultipleChoiceElement toggle in toggles)
            {
                toggle.SetButtonSprite(circleBubble);
                toggle.SetIndicatorSprite(circleFilled);
                toggle.SetSingleChoice(true);
            }
        }
        else if (maxAnswers > 1)
        {
            foreach (MultipleChoiceElement toggle in toggles)
            {
                toggle.SetButtonSprite(squareBubble);
                toggle.SetIndicatorSprite(squareFilled);
                toggle.SetSingleChoice(false);
            }
        }
    }

    public void SetChoices(string[] newChoices)
    {
        choices = newChoices;
    }
    public void OptionToggled()
    {
        int numActive = 0;

        foreach (MultipleChoiceElement toggle in toggles)
        {
            if (toggle.GetCurrentState())
                numActive++;
        }

        if (maxAnswers > 1)
        {
            if (numActive >= maxAnswers)
            {
                foreach (MultipleChoiceElement toggle in toggles)
                {
                    toggle.SetAbleToAdd(false);
                }
            }
            else
            {
                foreach (MultipleChoiceElement toggle in toggles)
                {
                    toggle.SetAbleToAdd(true);
                }
            }
        }
    }
    public void ClearAllButOne(MultipleChoiceElement exception)
    {
        foreach(MultipleChoiceElement toggle in toggles)
        {
            if(toggle != exception) 
            {
                toggle.SetCurrentState(false);
            }
        }
    }

    public MultipleChoiceElement[] GetAnswerValues()
    {
        return toggles;
    }
    public MultipleChoiceElement[] GetSelectedOptions()
    {
        int numSelected = 0;
        foreach(MultipleChoiceElement toggle in toggles)
        {
            if(toggle.GetCurrentState())
                numSelected++;
        }
        MultipleChoiceElement[] returnValue = new MultipleChoiceElement[numSelected];

        int index = 0;
        foreach(MultipleChoiceElement toggle in toggles)
        {
            if (toggle.GetCurrentState())
            {
                returnValue[index] = toggle;
                index++;
            }
        }

        return returnValue;
    }
}
