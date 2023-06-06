using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using UnityEditor;

public enum BuildTypes
{
    BuiltItem,
    Item,
    Floor,
    Player
}

public class InventoryTab : MonoBehaviour
{
    public string target;
    public BuildTypes type;
    public Transform scrollView;
    public List<InventoryItem> inventoryItems;
    public ItemTab buttonPrefab;
    private int index;
    private List<ItemTab> inventoryTabs = new List<ItemTab>();
    private void ClearTab()
    {
        foreach (Transform child in scrollView)
        {
            child.parent = null;
            Destroy(child.gameObject);
        }
        //inventoryItems.Clear();
        inventoryTabs.Clear();
    }
    public void AddTab(string id)
    {
        var t = Instantiate(buttonPrefab);
        t.transform.SetParent(scrollView);
        t.name = id;
        t.ID = id;
        inventoryTabs.Add(t);
        t.Render(false);
        var rectT = t.GetComponent<RectTransform>();
        rectT.offsetMax = new Vector2 (0, 0);
        rectT.offsetMin = new Vector2 (0, -30);
    }
    public void AddTab(InventoryItem tabItem)
    {
        var t = Instantiate(buttonPrefab);
        t.transform.SetParent(scrollView);
        t.name = tabItem.ID;
        t.ID = tabItem.ID;
        t.humanName = tabItem.humanName;
        t.count = tabItem.count;
        inventoryTabs.Add(t);
        t.Render(false);
        var rectT = t.GetComponent<RectTransform>();
        Vector2 offset = new Vector2(0, -30 * (scrollView.childCount-1));
        Debug.Log(t.ID + " " + offset);
        rectT.offsetMax = new Vector2(0, 0) + offset;
        rectT.offsetMin = new Vector2(0, -30) + offset;
        rectT = scrollView.GetComponent<RectTransform>();
        rectT.offsetMin = offset - new Vector2(0,30);
        
    }
    private void SelectTab(int tab)
    {
        for (int i = 0; i < inventoryTabs.Count; i++)
        {
            if(i == tab) { inventoryTabs[i].Render(true); }
            else { inventoryTabs[i].Render(false); }
        }
    }
    public void Arrange()
    {
        ClearTab();
        index = 0;
        inventoryItems = Player.currentPlayer.GetItemList();
        inventoryTabs = new List<ItemTab>();
        foreach (InventoryItem i in inventoryItems) 
        { 
            AddTab(i);

        }
        SelectTab(0);
    }

    private void IncIndex() { index++; }
    private void DecIndex() { index--; }
    private void ModIndex() { index = index % scrollView.childCount; }

}
