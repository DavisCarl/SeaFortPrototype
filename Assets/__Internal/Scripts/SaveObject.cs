using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveObject
{
    public string builtItemData, itemData, playerData, floorData, infoData;

    public SaveObject(string builtItemData, string itemData, string playerData, string floorData, string infoData)
    {
        this.builtItemData = builtItemData;
        this.itemData = itemData;
        this.playerData = playerData;
        this.floorData = floorData;
        this.infoData = infoData;
    }
    public SaveObject()
    {
        
    }
}
