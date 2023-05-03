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
    public string defaultPlayer;
    public Vector3 spawnPoint;
    public string savePath = "/Saves/";
    private List<BuiltItem> builtItems = new List<BuiltItem>();
    private List<Item> items = new List<Item>();
    private List<Floor> floors = new List<Floor>();
    private List<Player> players = new List<Player>();
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
    #region Save/Load
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
        floors[0].transform.localPosition = Vector3.zero;
        players.Add(Instantiate(playerDict[defaultPlayer]));
        players[0].transform.parent = transform;
        players[0].transform.position = spawnPoint;
    }
    public void Save()
    {
        SaveObject ob = new SaveObject();
        ob.builtItemData = Serialize(builtItems);
        ob.itemData = Serialize(items);
        ob.floorData = Serialize(floors);
        
        infoDict = new Dictionary<string, SaveInfoData>();
        foreach (StoredData d in GetComponentsInChildren<StoredData>())
        {
            infoDict.Add(d.personalID, d.Save());
        }
        ob.infoData = JsonConvert.SerializeObject(infoDict);
        File.WriteAllText(savePath + "MapData.json", JsonConvert.SerializeObject(ob));
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
        var data = JsonConvert.DeserializeObject<SaveObject>(File.ReadAllText(savePath + "MapData.json"));
        List<SavePosData> dataList = JsonConvert.DeserializeObject<List<SavePosData>>(data.floorData);
        infoDict = JsonConvert.DeserializeObject<Dictionary<string, SaveInfoData>>(data.infoData);
        players.Add(Instantiate(playerDict[defaultPlayer]));
        players[0].transform.parent = transform;
        players[0].transform.position = spawnPoint;
        foreach (SavePosData d in dataList)
        {
            var f = Instantiate(floorDict[d.ID]);
            floors.Add(f);
            f.transform.parent = transform;
            f.transform.SetPositionAndRotation(d.ToVector(), d.ToQuaternion());
         }
        dataList = JsonConvert.DeserializeObject<List<SavePosData>>(data.itemData);
        foreach (SavePosData d in dataList)
        {
            var f = Instantiate(itemDict[d.ID]);
            items.Add(f);
            
            f.transform.parent = transform;
            f.transform.SetPositionAndRotation(d.ToVector(), d.ToQuaternion());
            f.personalID = d.personalID;
        }
        dataList = JsonConvert.DeserializeObject<List<SavePosData>>(data.builtItemData);
        foreach (SavePosData d in dataList)
        {
            var f = Instantiate(builtItemDict[d.ID]);
            builtItems.Add(f);
            f.transform.parent = transform;
            f.transform.SetPositionAndRotation(d.ToVector(), d.ToQuaternion());
            f.personalID = d.personalID;
        }
    }
    #endregion
}
