using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBarterer
{
    public bool HasCredits(int count);
    public bool HasItem(string item, int count);
    public int ChangeCredits(int count);
    public void GiveItem(InventoryItem item);
    public void TakeItem(string item, int count);
}
