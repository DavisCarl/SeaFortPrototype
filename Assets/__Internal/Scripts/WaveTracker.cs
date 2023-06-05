using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class WaveTracker : MonoBehaviour
{
    private Transform t;
    // Start is called before the first frame update
    void Start()
    {
        t = transform;
        //enabled = false;
    }

    // Update is called once per frame
    public void UpdatePos()
    {
        t.position = new Vector3(t.position.x, WaterController.CalculateWave(t.position), t.position.z);
    }
}
