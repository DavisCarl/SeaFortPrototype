using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine.Jobs;

public class StaticWave
{
    
    public static float CalculateWave(Vector3 position, float speed, float waveScale, float waveDistance, float strength, float walk, float time, float waterHeight)
    {
        return WaveTypes.SinXWave(position: position, speed: speed, scale: waveScale, waveDistance: waveDistance, noiseStrength: strength, noiseWalk: walk, timeSinceStart: time) + waterHeight;

    }
}

public class WaveRenderer : MonoBehaviour
{
    [BurstCompile(CompileSynchronously =true)]
    struct UpdateWaveLevelJob : IJobParallelForTransform
    {
        [ReadOnly]
        public float speed, waveScale, waveDistance, strength, walk, time, waterHeight;
        // The code actually running on the job
        public void Execute(int index, TransformAccess transform)
        {
            // Move the transforms based on delta time and velocity
            var pos = transform.position;
            pos.y = StaticWave.CalculateWave(transform.position, speed, waveScale, waveDistance, strength, walk, time, waterHeight);  
            transform.position = pos;
        }
    }



    public Vector2Int size;
    public Transform waveTracker;
    private List<Transform> points = new List<Transform>();
    private List<float> heights = new List<float>();
    private bool threadConfigged = false;
    // Start is called before the first frame update
    void Start()
    {
        GameObject g = new GameObject("WaveRoot");
        //g.transform.parent = transform;
        g.transform.localPosition = Vector3.zero; 
        //for(int x = 0; x < size.x; x++) { for (int y = 0; y < size.y; y++) { var t = Instantiate(waveTracker); t.parent = g.transform; points.Add(t); var v = new Vector3(t.position.x + (x) - (size.y / 2), 0, t.position.z + (y) - (size.y / 2));t.position = v; } }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
