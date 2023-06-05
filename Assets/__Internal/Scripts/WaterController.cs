using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class WaveTypes
{
    //Sinus waves
    public static float SinXWave(
        Vector3 position,
        float speed,
        float scale,
        float waveDistance,
        float noiseStrength,
        float noiseWalk,
        float timeSinceStart)
    {
        float x = position.x;
        float y = 0f;
        float z = position.z;

        //Using only x or z will produce straight waves
        //Using only y will produce an up/down movement
        //x + y + z rolling waves
        //x * z produces a moving sea without rolling waves

        float waveType = z;

        y += Mathf.Sin((timeSinceStart * speed + waveType) / waveDistance) * scale;

        //Add noise to make it more realistic
        y += Mathf.PerlinNoise(x + noiseWalk, y + Mathf.Sin(timeSinceStart * 0.1f)) * noiseStrength;

        return y;
    }
}
public class WaterController : MonoBehaviour
{
    public WaveData waveData;
    public Material waveMaterial;
    public bool amplifyShader = true;
    public static float waveScale = 5;
    public static float waterHeight = 0;
    public static WaterController current;
    public static float rhoWater = 100;
    public static float speed = 5;
    public static float waveDistance = 15;
    public static float strength = 1;
    public static float walk = 1;
    private static float secondsSinceStart;
    
    // Start is called before the first frame update
    void Start()
    {
        current = this;
        SetWaveData(waveData);
    }

    public void SetWaveData(WaveData newWaveData)
    {
        waveScale = newWaveData.waveScale;
        waterHeight = newWaveData.waterHeight;
        rhoWater = newWaveData.rhoWater;
        speed = newWaveData.speed;
        waveDistance = newWaveData.waveDistance;
        strength = newWaveData.strength;
        walk = newWaveData.walk;
    }

    float h;
    Vector2 v;
    public static float GetWaterHeight(Vector3 position)
    {
        float h = CalculateWave(position);
        if (position.y > h) return -1;
        else return h - position.y;
    }
    
    public static float CalculateWave(Vector3 position) 
    {           
        return WaveTypes.SinXWave(position:position, speed: speed, scale: waveScale, waveDistance: waveDistance, noiseStrength: strength, noiseWalk: walk, timeSinceStart:secondsSinceStart) + waterHeight;

    }

    private void DoWave() {

        if (amplifyShader)
        {
            waveMaterial.SetFloat("_WaterScale", waveScale);
            waveMaterial.SetFloat("_WaterSpeed", speed);
            waveMaterial.SetFloat("_WaterDistance", waveDistance);
            waveMaterial.SetFloat("_WaterTime", Time.time);
            waveMaterial.SetFloat("_WaterNoiseStrength", strength);
            waveMaterial.SetFloat("_WaterNoiseWalk", walk);
            waveMaterial.SetFloat("_WaterHeight", waterHeight);
        }
        else {
            Shader.SetGlobalFloat("_WaterScale", waveScale);
            Shader.SetGlobalFloat("_WaterSpeed", speed);
            Shader.SetGlobalFloat("_WaterDistance", waveDistance);
            Shader.SetGlobalFloat("_WaterTime", Time.time);
            Shader.SetGlobalFloat("_WaterNoiseStrength", strength);
            Shader.SetGlobalFloat("_WaterNoiseWalk", walk);
            Shader.SetGlobalFloat("_WaterHeight", waterHeight);
        }
    }

    // Update is called once per frame
    void Update()
    {
        SetWaveData(waveData);
        secondsSinceStart = Time.time;
        DoWave();
    }
}
