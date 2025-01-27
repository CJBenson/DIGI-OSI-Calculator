using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SearchableMenu : MonoBehaviour
{
    [SerializeField] SearchMenuUnselectedItem resultTemplate;
    [SerializeField] Transform parentTransform;
    [SerializeField] TMP_InputField inputField;
    [SerializeField] SearchMenuSelectedSection selectedSection;
    [SerializeField] string[] manualInputOptions;
    List<SearchMenuUnselectedItem> unselectedKeywords = new List<SearchMenuUnselectedItem>();
    List<SearchMenuUnselectedItem> selectedKeywords = new List<SearchMenuUnselectedItem>();

    private void Start()
    {
        foreach(string option in manualInputOptions) 
        {
            LoadKeyword(option);
        }
    }
    private void Update()
    {
        string partialWord = inputField.text;

        foreach(SearchMenuUnselectedItem item in unselectedKeywords) 
        {
            if(item.GetKeyword().Contains(partialWord, System.StringComparison.CurrentCultureIgnoreCase))
            {
                item.gameObject.SetActive(true);
            }
            else
            {
                item.gameObject.SetActive(false);
            }
        }
    }

    public void LoadKeyword(string keyword)
    {
        GameObject itemGO = Instantiate(resultTemplate.gameObject, parentTransform);
        SearchMenuUnselectedItem item = itemGO.GetComponent<SearchMenuUnselectedItem>();
        item.Initialize(keyword);
        unselectedKeywords.Add(item);
    }
    public void SelectKeyword(SearchMenuUnselectedItem keyword)
    {
        Debug.Assert(unselectedKeywords.Contains(keyword), "Keyword is not in unselected list!");

        selectedKeywords.Add(keyword);
        unselectedKeywords.Remove(keyword);

        keyword.gameObject.SetActive(false);
        selectedSection.AddItem(keyword.GetKeyword());
    }
    public void UnselectKeyword(string keyword)
    {
        SearchMenuUnselectedItem item = selectedKeywords[0];
        int i = 1;
        while (i <= selectedKeywords.Count && item.GetKeyword() != keyword)
        {
            item = selectedKeywords[i];
            i++;
        }
        selectedKeywords.Remove(item);
        unselectedKeywords.Add(item);

        item.gameObject.SetActive(true);
    }

    public SearchMenuSelectedItem[] GetSelections()
    {
        return selectedSection.GetSelectedItems();
    }
}
