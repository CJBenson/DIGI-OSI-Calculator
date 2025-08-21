using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MultipleChoiceQuestion: MonoBehaviour
{
    [SerializeField] MultipleChoiceElement optionTemplate;

    // User edited
    [SerializeField][Tooltip("Maximum number of answers the user may select")] int maxAnswers;
    [SerializeField][Tooltip("Bubble/Fill sprites for questions with maxAnswers of 1")] Sprite singleAnswerBubble, singleAnswerFill;
    [SerializeField][Tooltip("Bubble/Fill sprites for questions with maxAnswers greater than 1")] Sprite multiAnswerBubble, multiAnswerFill;
    [SerializeField][Tooltip("The text for each selectable answer option")] string[] optionsText;


    MultipleChoiceElement[] answerOptions;
    List<MultipleChoiceElement> selectedOptions = new List<MultipleChoiceElement>();


    private void Start()
    { 
        // Create and populate the answer options array
        answerOptions = new MultipleChoiceElement[optionsText.Length];

        for (int i = 0; i < optionsText.Length; i++)
        {
            GameObject newOptionGO = Instantiate(optionTemplate.gameObject, transform);
            MultipleChoiceElement newOption = newOptionGO.GetComponent<MultipleChoiceElement>();
            newOption.SetQuestion(this);
            newOption.SetText(optionsText[i]);
            answerOptions[i] = newOption;

            newOptionGO.SetActive(true);
        }

        // Set the bubble/fill sprites based on whether it is a single- or multi-answer question
        maxAnswers = Mathf.Clamp(maxAnswers, 1, answerOptions.Length);

        Sprite bubbleToUse = maxAnswers > 1 ? multiAnswerBubble : singleAnswerBubble;
        Sprite fillToUse = maxAnswers > 1 ? multiAnswerFill : singleAnswerFill;

        foreach (MultipleChoiceElement currentOption in answerOptions)
        {
            currentOption.SetButtonSprite(bubbleToUse);
            currentOption.SetIndicatorSprite(fillToUse);
        }
    }

    public void SetChoicesText(string[] textsIn)
    {
        optionsText = textsIn;
    }


    public void OptionToggled(MultipleChoiceElement toggledOption)
    {
        // Determine whether user is adding or removing a selection
        if (toggledOption.GetSelectedState())
            selectedOptions.Add(toggledOption);
        else
            selectedOptions.Remove(toggledOption);


        if (maxAnswers > 1)
        {
            int numSelected = 0;

            foreach (MultipleChoiceElement currentOption in answerOptions)
            {
                if (currentOption.GetSelectedState())
                    numSelected++;

                currentOption.SetSelectable(numSelected < maxAnswers);
            }
        }
        else
        {
            foreach (MultipleChoiceElement currentOption in answerOptions)
            {
                // The toggled option does not have to be selected--users should be able to unselect even in a single-choice
                if (currentOption != toggledOption)
                {
                    currentOption.SetSelectedState(false);

                    if (selectedOptions.Contains(currentOption))
                        selectedOptions.Remove(currentOption);
                }
                
            }
        }
    }

    public MultipleChoiceElement[] GetAnswerOptions()
    {
        return answerOptions;
    }
    public MultipleChoiceElement[] GetSelectedOptions()
    {
        return selectedOptions.ToArray();
    }
}
