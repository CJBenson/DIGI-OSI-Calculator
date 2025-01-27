using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MultipleChoiceElement : MonoBehaviour
{
    [SerializeField] bool startingState = false;
    [SerializeField] Image selectionButton;
    [SerializeField] Image selectedIndicator;
    [SerializeField] TMP_Text choiceText;
    [SerializeField] MultipleChoiceQuestion question;
    bool singleChoice;
    bool currentState;
    bool ableToAdd = true;

    private void Start()
    {
        currentState = startingState;
    }

    //Setting the parameters of the element
    public void SetSingleChoice(bool singleChoiceAssignment)
    {
        singleChoice = singleChoiceAssignment;
    }
    public void SetAbleToAdd(bool ableToAddAssignment)
    {
        ableToAdd = ableToAddAssignment;
    }
    public void SetButtonSprite(Sprite spriteAssignment)
    {
        selectionButton.sprite = spriteAssignment;
    }
    public void SetIndicatorSprite(Sprite spriteAssignment)
    {
        selectedIndicator.sprite = spriteAssignment;
    }
    public void SetText(string text)
    {
        choiceText.text = text;
    }
    public string GetText()
    {
        return choiceText.text;
    }

    //Setting/Getting the state of the answer
    public bool GetCurrentState()
    {
        return currentState;
    }
    public void SetCurrentState(bool state)
    {
        currentState = state;
        selectedIndicator.enabled = currentState;
    }
    public void Toggle()
    {
        if (ableToAdd || currentState)
        {
            currentState = !currentState;
            selectedIndicator.enabled = currentState;
        }
        if (singleChoice)
            question.ClearAllButOne(this);
    }
}
