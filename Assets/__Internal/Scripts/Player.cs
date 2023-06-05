using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryItem
{
    public string ID;
    public string buildID;
    public string humanName;
    public int count;
    public InventoryItem(PlantStats plant)
    {
        ID = plant.cropID;
        buildID = plant.buildID;
        count = plant.cropSize;
        humanName = plant.cropHumanName;
    }
    public InventoryItem(Item item)
    {
        ID = item.ID;
        count = 1;
        humanName = item.humanName;
    }
    public InventoryItem() { }
}
[System.Serializable]
public class PlayerData
{
    public int health = 100;
    public int maxHealth = 100;
    public int credits;
    public List<InventoryItem> items = new List<InventoryItem>();
    public PlayerData() { }
}

public class Player : MonoBehaviour, IIdentifiable, IStoredData, IBarterer
{
    public static Player currentPlayer;
    #region Interfaces
    [SerializeField]
    private string id;
    [SerializeField]
    private PlayerData stats;
    [SerializeField]
    private InventoryTab buildTab;
    public string ID
    {
        get { return id; }
        set { id = value; }
    }
    public string personalID
    {
        get;
        set;
    }
    public Transform Transform
    {
        get { return transform; }
    }
    public void Init()
    {
        if (personalID == null)
        {
            personalID = "Player-" + transform.root.childCount;
        }
        foreach (IStoredData sd in transform.GetComponentsInChildren<IStoredData>())
        {
            sd.personalID = personalID;
        }
        WorldRoot root = FindObjectOfType<WorldRoot>();
        root.Register(this);
        currentPlayer = this;
    }

    public void Load(SaveInfoData d)
    {
        stats = JsonConvert.DeserializeObject<PlayerData>(d.serializedData);
        currentPlayer = this;
        buildTab.Arrange();
    }

    public SaveInfoData Save()
    {
        SaveInfoData d = new SaveInfoData();
        d.personalID = personalID;
        d.serializedData = JsonConvert.SerializeObject(stats);
        return d;
    }
    #endregion
    #region Inventory Actions
    public void GiveItem(InventoryItem itemToAdd)
    {
        var i = stats.items.FindIndex(o => o.ID == itemToAdd.ID);
        Debug.Log(i);
        if (i > -1)
        {
            stats.items[i].count += itemToAdd.count;
        }
        else
        {
            stats.items.Add(itemToAdd);
        }
        buildTab.Arrange();
    }
    public bool HasItem(string itemID, int count)
    {
        bool has = false;
        var i = stats.items.FindIndex(o => o.ID == itemID);
        if (i > -1)
        {
            has = stats.items[i].count >= count;
        }

        return has;
    }
    public bool HasCredits(int count)
    {
        if (stats.credits >= count) { return true; } else { return false; }
    }

    public int ChangeCredits(int count)
    {
        stats.credits = Mathf.Max(stats.credits + count, 0);
        return stats.credits;
    }

    public void TakeItem(string itemID, int count)
    {
        var i = stats.items.FindIndex(o => o.ID == itemID);
        if (i > -1)
        {
            stats.items[i].count -= count;
        }
        buildTab.Arrange();
    }
    public List<InventoryItem> GetItemList()
    {
        return stats.items;
    }
    #endregion
    #region Unity Behaviours
    void Start()
    {
        currentPlayer = this;
    }
    #endregion
    
}
