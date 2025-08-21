using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MultipleChoiceElement : MonoBehaviour
{
    [SerializeField] bool startsSelected = false;

    [SerializeField] Image selectionBubble;
    [SerializeField] Image selectedIndicator;
    [SerializeField] TMP_Text optionText;

    MultipleChoiceQuestion question;

    bool currentlySelected;
    bool selectable = true;

    private void Start()
    {
        currentlySelected = startsSelected;
    }

    public void SetQuestion(MultipleChoiceQuestion questionIn)
    {
        question = questionIn;
    }
    public void SetSelectable(bool selectableIn)
    {
        selectable = selectableIn;
    }
    public void SetButtonSprite(Sprite spriteIn)
    {
        selectionBubble.sprite = spriteIn;
    }
    public void SetIndicatorSprite(Sprite spriteIn)
    {
        selectedIndicator.sprite = spriteIn;
    }

    public string GetText()
    {
        return optionText.text;
    }
    public void SetText(string textIn)
    {
        optionText.text = textIn;
    }

    public bool GetSelectedState()
    {
        return currentlySelected;
    }
    public void SetSelectedState(bool stateIn)
    {
        currentlySelected = stateIn;
        selectedIndicator.enabled = currentlySelected;
    }
    public void ToggleSelectedState()
    {
        if (selectable || currentlySelected)
            SetSelectedState(!currentlySelected);

        question.OptionToggled(this);
    }
}
