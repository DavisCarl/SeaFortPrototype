using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IShipComponent  
{
    public string ID { get; set; }
    public string Save();
    public void Load(string savedData);
}
