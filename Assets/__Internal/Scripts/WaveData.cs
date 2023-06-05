using UnityEngine;

[CreateAssetMenu(menuName = "WaterInfo/WaveData")]
public class WaveData : ScriptableObject 
{
    public float waveScale = 5;
    public float waterHeight = 0;
    public float rhoWater = 100;
    public float speed = 5;
    public float waveDistance = 15;
    public float strength = 1;
    public float walk = 1;
}
