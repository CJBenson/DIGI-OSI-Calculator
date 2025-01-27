using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchMenuSelectedSection : MonoBehaviour
{
    [SerializeField] GameObject menuItemTemplate;
    [SerializeField] SearchableMenu menu;
    private List<SearchMenuSelectedItem> selectedItems = new List<SearchMenuSelectedItem>();

    public void AddItem(string keyword)
    {
        GameObject newItemGO = Instantiate(menuItemTemplate, transform);
        SearchMenuSelectedItem newItem = newItemGO.GetComponent<SearchMenuSelectedItem>();
        newItem.Initialize(keyword);
        selectedItems.Add(newItem);
        newItemGO.SetActive(true);
    }
    public void RemoveItem(SearchMenuSelectedItem toBeRemoved)
    {
        selectedItems.Remove(toBeRemoved);
        Destroy(toBeRemoved.gameObject);

        menu.UnselectKeyword(toBeRemoved.GetKeyword());
    }

    public SearchMenuSelectedItem[] GetSelectedItems()
    {
        return selectedItems.ToArray();
    }
}
