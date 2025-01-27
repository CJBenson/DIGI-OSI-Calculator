using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YesNoOption : MonoBehaviour
{
    [SerializeField] GameObject optionalQuestion;

    public void CheckAnswer()
    {
        MultipleChoiceQuestion questionData = gameObject.GetComponent<MultipleChoiceQuestion>();
        MultipleChoiceElement enableElement = questionData.GetAnswerValues()[0];

        if (enableElement.GetCurrentState())
            optionalQuestion.SetActive(true);
        else 
            optionalQuestion.SetActive(false);
    }
}
