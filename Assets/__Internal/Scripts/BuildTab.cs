using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuildTab : MonoBehaviour
{
    public string target;
    public Transform scrollView;
    public List<BuiltItem> inventoryItems;
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
        inventoryItems.Clear();
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
        Vector2 offset = new Vector2(0, -30 * (scrollView.childCount - 1));
        rectT.offsetMax = new Vector2(0, 0) + offset;
        rectT.offsetMin = new Vector2(0, -30) + offset;
        rectT = scrollView.GetComponent<RectTransform>();
        rectT.offsetMin = offset - new Vector2(0, 30);
    }
    public void AddTab(BuiltItem i)
    {
        var t = Instantiate(buttonPrefab);
        t.transform.SetParent(scrollView);
        t.name = i.ID;
        t.ID = i.ID;
        t.humanName = i.humanName;
        inventoryTabs.Add(t);
        t.Render(false);
        var rectT = t.GetComponent<RectTransform>();
        Vector2 offset = new Vector2(0, -30 * (scrollView.childCount - 1));
        rectT.offsetMax = new Vector2(0, 0) + offset;
        rectT.offsetMin = new Vector2(0, -30) + offset;
        rectT = scrollView.GetComponent<RectTransform>();
        rectT.offsetMin = offset - new Vector2(0, 30);
    }
    private void SelectTab(int tab)
    {
        for (int i = 0; i < inventoryTabs.Count; i++)
        {
            if (i == tab) { inventoryTabs[i].Render(true); SetBuildTarget(inventoryTabs[i].ID); }
            else { inventoryTabs[i].Render(false); }
        }
    }
    public void Arrange()
    {
        ClearTab();
        index = 0;
        inventoryItems = WorldRoot.Instance.GetBuiltItemDict().Values.ToList();
        inventoryTabs = new List<ItemTab>();
        foreach (BuiltItem i in inventoryItems)
        {
            AddTab(i.ID);

        }
        SelectTab(0);
    }

    private void IncIndex() { index++; }
    private void DecIndex() { index--; }
    private void ModIndex() { index = index % scrollView.childCount; }

    public void SetBuildTarget(string target)
    {
        PlayerInput.builtItemToBuild = target;
    }
}