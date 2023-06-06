using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class WorldRoot : MonoBehaviour
{
    #region Public Vars
    public float floorHeight;
    public string defaultFloor;
    public string defaultPlayer;
    public Vector3 spawnPoint;
    public string savePath = "/Saves/";
    public bool ticking = true;
    public int tickRate = 20;
    public static WorldRoot Instance { get; private set; }
    #endregion
    #region Private Vars
    private int tick = 0;
    private List<BuiltItem> builtItems = new List<BuiltItem>();
    private List<Item> items = new List<Item>();
    private List<Floor> floors = new List<Floor>();
    private List<Player> players = new List<Player>();
    private List<ITickable> tickables = new List<ITickable>();
    private Dictionary<string, BuiltItem> builtItemDict = new Dictionary<string, BuiltItem>();
    private Dictionary<string, Item> itemDict = new Dictionary<string, Item>();
    private Dictionary<string, Floor> floorDict = new Dictionary<string, Floor>();
    private Dictionary<string, Player> playerDict = new Dictionary<string, Player>();
    private Dictionary<string, SaveInfoData> infoDict = new Dictionary<string, SaveInfoData>();
    #endregion
    private void Start()
    {
#if UNITY_WEBGL
        savePath = Application.persistentDataPath + savePath;
#endif
#if !UNITY_WEBGL
        savePath = Application.dataPath + savePath;
#endif
        Instance = this;
        var buildables = Resources.LoadAll("BuiltItems", typeof(BuiltItem)).ToArray();
        foreach (BuiltItem t in buildables)
        {
            //Debug.Log(t.ID);
            builtItemDict.Add(t.ID, t);
        }
        var itemTypes = Resources.LoadAll("Items", typeof(Item)).ToArray();
        foreach (Item t in itemTypes)
        {
            //Debug.Log(t.ID);
            itemDict.Add(t.ID, t);
        }
        var floorPlans = Resources.LoadAll("Floors", typeof(Floor)).ToArray();
        foreach (Floor t in floorPlans)
        {
            //Debug.Log(t.ID);
            floorDict.Add(t.ID, t);
        }
        var playerTypes = Resources.LoadAll("Players", typeof(Player)).ToArray();
        foreach (Player t in playerTypes)
        {
            //Debug.Log(t.ID);
            playerDict.Add(t.ID, t);
        }
    }
    private void Update()
    {
        if (ticking)
        {
            tick++;

            if (tick > tickRate)
            {
                foreach (ITickable t in tickables)
                {
                    t.Tick();
                }
                tick = 0;
            }
        }
    }
    #region Construction
    public Transform GetBuiltItem(string itemName)
    {
        return Instantiate(builtItemDict[itemName]).transform;
    }
    public Transform GetItem(string itemName)
    {
        return Instantiate(itemDict[itemName]).transform;
    }
    public void Register(ITickable value)
    {
        tickables.Add(value);
    }
    public void Deregister(ITickable value)
    {
        tickables.Remove(value);
    }
    public void Register(BuiltItem value)
    {
        builtItems.Add(value);
    }
    public void Register(Item value)
    {
        items.Add(value);
    }
    public void Register(Floor value)
    {
        floors.Add(value);
    }
    public void Register(Player value)
    {
        players.Add(value);
    }
    #endregion
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
        foreach (var t in players)
        {
            Destroy(t.gameObject);
        }
        items = new List<Item>();
        builtItems = new List<BuiltItem>();
        floors = new List<Floor>();
        players = new List<Player>();
        tickables = new List<ITickable>();
    }
    public void AddFloor(string floorType)
    {
        var f = (Instantiate(floorDict[floorType]));
        f.transform.parent = transform;
        f.transform.localPosition = Vector3.up * floorHeight * (floors.Count);
        f.Init();
    }
    public void AddPlayer(string playerType)
    {
        var p = (Instantiate(playerDict[playerType]));
        p.transform.parent = transform;
        p.transform.position = spawnPoint;
        p.Init();
    }
    public void New()
    {
        ClearWorld();
        AddFloor(defaultFloor);
        AddPlayer(defaultPlayer);
    }
    public void Save()
    {
        if (Directory.Exists(savePath)) { }
        else { Directory.CreateDirectory(savePath); } 
        SaveObject ob = new SaveObject();
        ob.builtItemData = Serialize(builtItems);
        ob.itemData = Serialize(items);
        ob.floorData = Serialize(floors);
        ob.playerData = Serialize(players);
        infoDict = new Dictionary<string, SaveInfoData>();
        foreach (IStoredData d in GetComponentsInChildren<IStoredData>())
        {
            infoDict.Add(d.personalID, d.Save());
        }
        ob.infoData = JsonConvert.SerializeObject(infoDict);
        File.WriteAllText(savePath + "MapData.json", JsonConvert.SerializeObject(ob));
        PlayerPrefs.SetString("forceSave", string.Empty);
        PlayerPrefs.Save();
    }
    public string Serialize(List<BuiltItem> items)
    {

        List<SavePosData> data = new List<SavePosData>();
        foreach (BuiltItem i in items)
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
    public string Serialize(List<Player> items)
    {

        List<SavePosData> data = new List<SavePosData>();
        foreach (Player i in items)
        {
            data.Add(new SavePosData(i));
        }
        string output = JsonConvert.SerializeObject(data);
        return output;
    }
    public Dictionary<string, Item> GetItemDict() { return itemDict; }
    public Dictionary<string, BuiltItem> GetBuiltItemDict() { return builtItemDict; }
    public void Load()
    {
        ClearWorld();
        var data = JsonConvert.DeserializeObject<SaveObject>(File.ReadAllText(savePath + "MapData.json"));
        infoDict = JsonConvert.DeserializeObject<Dictionary<string, SaveInfoData>>(data.infoData);
        List<SavePosData> dataList = JsonConvert.DeserializeObject<List<SavePosData>>(data.floorData);
        InstantiateList(dataList, BuildTypes.Floor);
        dataList = JsonConvert.DeserializeObject<List<SavePosData>>(data.itemData);
        InstantiateList(dataList, BuildTypes.Item);
        dataList = JsonConvert.DeserializeObject<List<SavePosData>>(data.builtItemData);
        InstantiateList(dataList, BuildTypes.BuiltItem);
        dataList = JsonConvert.DeserializeObject<List<SavePosData>>(data.playerData);
        InstantiateList(dataList, BuildTypes.Player);

    }
    void InstantiateList(List<SavePosData> dataList, BuildTypes currentType)
    {
        foreach (SavePosData d in dataList)
        {

            InstantiateData(d, currentType);
        }
    }
    void InstantiateData(SavePosData d, BuildTypes currentType)
    {
        Transform target = transform;
        switch (currentType)
        {
            case BuildTypes.Player:
                var p = Instantiate(playerDict[d.ID]);
                p.personalID = d.personalID;
                players.Add(p);
                FinishLoad(p.transform, d.ToVector(), d.ToQuaternion(), d.personalID);
                break;
            case BuildTypes.BuiltItem:
                var b = Instantiate(builtItemDict[d.ID]);
                b.personalID = d.personalID;
                builtItems.Add(b);
                FinishLoad(b.transform, d.ToVector(), d.ToQuaternion(), d.personalID);
                break;
            case BuildTypes.Item:
                var i = Instantiate(itemDict[d.ID]);
                i.personalID = d.personalID;
                items.Add(i);
                FinishLoad(i.transform, d.ToVector(), d.ToQuaternion(), d.personalID);
                break;
            case BuildTypes.Floor:
                var f = Instantiate(floorDict[d.ID]);
                f.personalID = d.personalID;
                floors.Add(f);
                FinishLoad(f.transform, d.ToVector(), d.ToQuaternion(), d.personalID);
                break;
        }
    }
    void FinishLoad(Transform target, Vector3 v, Quaternion q, string ID)
    {
        target.parent = transform;
        target.SetPositionAndRotation(v, q);

        foreach (IStoredData sd in target.GetComponentsInChildren<IStoredData>())
        {
            sd.personalID =  ID;
            sd.Load(infoDict[sd.personalID]);
        }
    }
    #endregion
}
