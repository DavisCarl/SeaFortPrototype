using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class PlantStats
{
    public int currentTick, stage;
    public int[] growthTicks;
    public int cropSize = 5;
    public string cropID, buildID, humanName, cropHumanName;
}

public class PlantBase : MonoBehaviour, IStoredData, IInteractable, ITickable
{
    public string personalID { get; set; }
    [SerializeField]
    private PlantStats stats;
    [SerializeField]
    private Transform[] stages;
    public void Load(SaveInfoData d)
    {
        stats = JsonConvert.DeserializeObject<PlantStats>(d.serializedData);
        Render();
    }

    public SaveInfoData Save()
    {
        SaveInfoData d = new SaveInfoData();
        d.personalID = personalID;
        d.serializedData = JsonConvert.SerializeObject(stats);
        return d;
    }

    // Start is called before the first frame update
    void Start()
    {
        WorldRoot root = FindObjectOfType<WorldRoot>();
        root.Register(this);
    }

    bool Harvest(Transform t)
    {
        if (stats.stage >= stats.growthTicks.Length)
        {
            stats.currentTick = 0;
            stats.stage = 0;
            for (int i = 0; i < stages.Length; i++)
            {
                stages[i].gameObject.SetActive(false);
            }
            t.GetComponent<Player>().GiveItem(new InventoryItem(stats));
            Render();
            return true;
        }
        return false;
    }

    private void Render()
    {
        for (int i = 0; i < stages.Length; i++)
        {
            if (i == stats.stage - 1) { stages[i].gameObject.SetActive(true); }
            else { stages[i].gameObject.SetActive(false); }
        }
    }

    public bool Execute(Transform t)
    {
        return Harvest(t);
    }

    public void Tick()
    {
        if (stats.stage < stats.growthTicks.Length)
        {
            stats.currentTick++;
            if (stats.currentTick > stats.growthTicks[stats.stage])
            {
                stats.currentTick = 0;
                stats.stage++;
                Render();
            }
        }
    }
}
