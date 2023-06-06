using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameCore : MonoBehaviour
{
    #region Public Vars
    public string defaultPlayer;
    public Vector3 spawnPoint;
    public string savePath = "/Saves/";
    public bool ticking = true;
    public int tickRate = 20;
    public static GameCore Instance { get; private set; }
    #endregion
    #region Private Vars
    private int tick = 0;
    private static Dictionary<string, BuiltItem> builtItemDict = new Dictionary<string, BuiltItem>();
    private static Dictionary<string, Item> itemDict = new Dictionary<string, Item>();
    private static Dictionary<string, Floor> floorDict = new Dictionary<string, Floor>();
    private static Dictionary<string, Player> playerDict = new Dictionary<string, Player>();
    private static Dictionary<string, SaveInfoData> infoDict = new Dictionary<string, SaveInfoData>();
    #endregion
    public static Dictionary<string, BuiltItem> GetBuiltItemDict() { return builtItemDict; }
    public static Dictionary<string, Player> GetPlayerDict() { return playerDict; }
    public static string playerDefault;
    // Start is called before the first frame update
    void Start()
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
        playerDefault = defaultPlayer;
    }

    // Update is called once per frame
    void Update()
    {

    }
}