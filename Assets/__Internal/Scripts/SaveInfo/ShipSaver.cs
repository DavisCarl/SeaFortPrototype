using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System.Linq;

public class ShipSaveObject
{
    public string ID;
    public Dictionary<string, string> componentData;
    public Dictionary<string, SaveInfoData> infoDict;
    public string serializedPlayers;
    public Vector3Data location;
    public QuaternionData rotation;
    public ShipSaveObject() { }
}

public class ShipSaver : MonoBehaviour
{
    [SerializeField]
    private Transform hullTransform, buoyancyTransform;
    [SerializeField]
    private BoatRoot boat;
    [SerializeField]
    private string savePath;
    [SerializeField]
    private string shipID;
    [SerializeField]
    private Vector3 spawnPoint = new Vector3(0, 10, 0);
    private Dictionary<string, string> componentData = new Dictionary<string, string>();
    private Dictionary<string, SaveInfoData> infoDict;
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
    public Vector3 GetSpawn() { return spawnPoint; }   
    void Save() 
    {
        
        componentData.Clear();
        foreach(IShipComponent s in GetComponentsInChildren<IShipComponent>())
        {
            componentData.Add(s.ID, s.Save());
        }
        if (Directory.Exists(savePath)) { }
        else { Directory.CreateDirectory(savePath); }
        ShipSaveObject ob = new ShipSaveObject();
        ob.componentData = componentData;
        infoDict = new Dictionary<string, SaveInfoData>();
        foreach (IStoredData d in boat.hullTransform.GetComponentsInChildren<IStoredData>())
        {
            infoDict.Add(d.personalID, d.Save());
        }
        ob.infoDict =  infoDict;
        ob.serializedPlayers = Serialize(boat.hullTransform.GetComponentsInChildren<Player>().ToList());
        Debug.Log(ob.serializedPlayers);
        ob.location = new Vector3Data(boat.buoyancyTransform.position);
        ob.rotation = new QuaternionData(boat.buoyancyTransform.rotation);
        File.WriteAllText(savePath + string.Format("/ShipData_{0}.json", shipID), JsonConvert.SerializeObject(ob));
        PlayerPrefs.SetString("forceSave", string.Empty);
        PlayerPrefs.Save();
    }
    IEnumerator Load()
    {
        if (File.Exists(savePath + string.Format("/ShipData_{0}.json", shipID)))
        {
            //boat.hullTransform = Instantiate(boat.hullTransform);
            //boat.buoyancyTransform = Instantiate(boat.buoyancyTransform);
            boat.hullTransform.GetComponent<ShipBuoyancyFollower>().buoyancyObject = boat.buoyancyTransform.GetComponent<Rigidbody>();
            
            var data = JsonConvert.DeserializeObject<ShipSaveObject>(File.ReadAllText(savePath + string.Format("/ShipData_{0}.json", shipID)));
            infoDict = data.infoDict;
            boat.buoyancyTransform.position = data.location.ToVector3();
            boat.buoyancyTransform.rotation = data.rotation.ToQuaternion();
            componentData = data.componentData;
            yield return new WaitForEndOfFrame();
            List<SavePosData> dataList = JsonConvert.DeserializeObject<List<SavePosData>>(data.serializedPlayers);
            InstantiateList(dataList, BuildTypes.Player); 
        }
        else { Default(); }
    }
    private IEnumerator Start()
    {
#if UNITY_WEBGL
        savePath = Application.persistentDataPath + savePath;
#endif
#if !UNITY_WEBGL
        savePath = Application.dataPath + savePath;
#endif
        boat = Instantiate(boat);
        yield return new WaitForEndOfFrame();
        StartCoroutine(Load());
    }
    void Default()
    {
        hullTransform = boat.hullTransform;
        buoyancyTransform = boat.buoyancyTransform;
        boat.hullTransform.GetComponent<ShipBuoyancyFollower>().buoyancyObject = boat.buoyancyTransform.GetComponent<Rigidbody>();
        AddPlayer(GameCore.playerDefault);
    }
    public void AddPlayer(string playerType)
    {
        var p = (Instantiate(GameCore.GetPlayerDict()[playerType]));
        p.transform.parent = boat.hullTransform;
        p.transform.localPosition = spawnPoint;
        p.Init();
    }
    void InstantiateList(List<SavePosData> dataList, BuildTypes currentType)
    {
        foreach (SavePosData d in dataList)
        {

            InstantiateData(d, currentType);
        }
    }
    private void OnDisable()
    {
        Save();
    }
    private void OnApplicationQuit()
    {
        Save();
    }
    void InstantiateData(SavePosData d, BuildTypes currentType)
    {
        Transform target = transform;
        switch (currentType)
        {
            case BuildTypes.Player:
                var p = Instantiate(GameCore.GetPlayerDict()[d.ID]);
                p.personalID = d.personalID;
                FinishLoad(p.transform, d.ToVector(), d.ToQuaternion(), d.personalID);
                break;
            case BuildTypes.BuiltItem:
                var b = Instantiate(GameCore.GetBuiltItemDict()[d.ID]);
                b.personalID = d.personalID;
                FinishLoad(b.transform, d.ToVector(), d.ToQuaternion(), d.personalID);
                break;
            case BuildTypes.Item:
                
                break;
            case BuildTypes.Floor:
                
                break;
        }
    }
    void FinishLoad(Transform target, Vector3 v, Quaternion q, string ID)
    {
        target.parent = boat.hullTransform;
        target.SetPositionAndRotation(v, q);
        foreach (IStoredData sd in target.GetComponentsInChildren<IStoredData>())
        {
            sd.personalID = ID;
            Debug.Log(ID);
            sd.Load(infoDict[sd.personalID]);
        }
    }
}
