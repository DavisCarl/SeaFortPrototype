using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.Linq;
using System.IO;

public class WorldRoot : MonoBehaviour
{
    public float floorHeight;
    public string defaultFloor;
    public string savePath = "/Saves/";
    private List<BuiltItem> builtItems = new List<BuiltItem>();
    private List<Item> items = new List<Item>();
    private List<Floor> floors = new List<Floor>();
    private Dictionary<string, BuiltItem> builtItemDict = new Dictionary<string, BuiltItem>();
    private Dictionary<string, Item> itemDict = new Dictionary<string, Item>();
    private Dictionary<string, Floor> floorDict = new Dictionary<string, Floor>();
    private Dictionary<string, Player> playerDict = new Dictionary<string, Player>();
    private Dictionary<string, SaveInfoData> infoDict = new Dictionary<string, SaveInfoData>();
    private void Start()
    {
        savePath = Application.dataPath + savePath;
        var buildables = Resources.LoadAll("BuiltItems", typeof(BuiltItem)).ToArray();
        foreach (BuiltItem t in buildables)
        {
            Debug.Log(t.ID);
            builtItemDict.Add(t.ID, t);
        }
        var itemTypes = Resources.LoadAll("Items", typeof(Item)).ToArray();
        foreach (Item t in itemTypes)
        {
            Debug.Log(t.ID);
            itemDict.Add(t.ID, t);
        }
        var floorPlans = Resources.LoadAll("Floors", typeof(Floor)).ToArray();
        foreach (Floor t in floorPlans)
        {
            Debug.Log(t.ID);
            floorDict.Add(t.ID, t);
        }
        var playerTypes = Resources.LoadAll("Players", typeof(Player)).ToArray();
        foreach (Player t in playerTypes)
        {
            Debug.Log(t.ID);
            playerDict.Add(t.ID, t);
        }
    }

    private void ClearWorld()
    {
        foreach (var t in builtItems)
        {
            Destroy(t.gameObject);
        }
        foreach (var t in items)
        {
            Destroy(t.gameObject);
        }
        foreach (var t in floors)
        {
            Destroy(t.gameObject);
        }
        items = new List<Item>();
        builtItems = new List<BuiltItem>();
        floors = new List<Floor>();
    }

    public void New()
    {
        ClearWorld();
        floors.Add(Instantiate(floorDict[defaultFloor]));
        floors[0].transform.parent = transform;
    }
    public void Save()
    {
        Save(builtItems);
        Save(items);
        Save(floors);
        infoDict = new Dictionary<string, SaveInfoData>();
        foreach (StoredData d in GetComponentsInChildren<StoredData>())
        {
            infoDict.Add(d.personalID, d.Save());
        }
        Save(infoDict);
    }
    private void Save(List<BuiltItem> items)
    {
        File.WriteAllText(savePath + "BuiltItems.json", Serialize(items));
    }
    private void Save(Dictionary<string, SaveInfoData> info)
    {
        File.WriteAllText(savePath + "Info.json", JsonConvert.SerializeObject(info));
    }
    public string Serialize(List<BuiltItem> items)
    {
        
        List<SavePosData> data = new List<SavePosData>();
        foreach(BuiltItem i in items) 
        {
            data.Add(new SavePosData(i));
        }
        string output = JsonConvert.SerializeObject(data);
        return output;
    }
    private void Save(List<Item> items)
    {
        File.WriteAllText(savePath + "Items.json", Serialize(items));
    }
    public string Serialize(List<Item> items)
    {

        List<SavePosData> data = new List<SavePosData>();
        foreach (Item i in items)
        {
            data.Add(new SavePosData(i));
        }
        string output = JsonConvert.SerializeObject(data);
        return output;
    }
    private void Save(List<Floor> items)
    {
        File.WriteAllText(savePath + "Floors.json", Serialize(items));
    }
    public string Serialize(List<Floor> items)
    {

        List<SavePosData> data = new List<SavePosData>();
        foreach (Floor i in items)
        {
            data.Add(new SavePosData(i));
        }
        string output = JsonConvert.SerializeObject(data);
        return output;
    }
    public void Load()
    {
        ClearWorld();
        List<SavePosData> data = JsonConvert.DeserializeObject<List<SavePosData>>(File.ReadAllText(savePath + "Floors.json"));
        infoDict = JsonConvert.DeserializeObject<Dictionary<string, SaveInfoData>>(savePath + "Info.json");
        foreach (SavePosData d in data)
        {
            var f = Instantiate(floorDict[d.ID]);
            floors.Add(f);
            f.transform.parent = transform;
            f.transform.SetPositionAndRotation(d.ToVector(), d.ToQuaternion());
         }
        data = JsonConvert.DeserializeObject<List<SavePosData>>(File.ReadAllText(savePath + "Items.json"));
        foreach (SavePosData d in data)
        {
            var f = Instantiate(itemDict[d.ID]);
            items.Add(f);
            
            f.transform.parent = transform;
            f.transform.SetPositionAndRotation(d.ToVector(), d.ToQuaternion());
            f.personalID = d.personalID;
        }
        data = JsonConvert.DeserializeObject<List<SavePosData>>(File.ReadAllText(savePath + "BuiltItems.json"));
        foreach (SavePosData d in data)
        {
            var f = Instantiate(builtItemDict[d.ID]);
            builtItems.Add(f);
            f.transform.parent = transform;
            f.transform.SetPositionAndRotation(d.ToVector(), d.ToQuaternion());
            f.personalID = d.personalID;
        }
    }

}
