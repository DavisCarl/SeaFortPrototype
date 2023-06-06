using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine.Jobs;
using UnityEngine.UIElements;

public struct TriangleData
{
    public static Vector3 Center(Vector3 p1, Vector3 p2, Vector3 p3) 
    { 
        return (p1 + p2 + p3) / 3f;
    }
    public static float Area(Vector3 p1, Vector3 p2, Vector3 p3) 
    { 
        float a = Vector3.Distance(p1, p2);

        float c = Vector3.Distance(p3, p1);

        return (a * c * Mathf.Sin(Vector3.Angle(p2 - p1, p3 - p1) * Mathf.Deg2Rad)) / 2f;
    }
    public static Vector3 Normal(Vector3 p1, Vector3 p2, Vector3 p3) 
    {      
        return Vector3.Cross(p2 - p1, p3 - p1).normalized;
    }
    public static Vector3 Normal(Vector3 normal, Transform t)
    {
        return t.TransformVector(normal);
    }
}
public class BuoyantObject : MonoBehaviour
{
    struct WaterHeightJob : IJobParallelFor
    {
        // Jobs declare all data that will be accessed in the job
        // By declaring it as read only, multiple jobs are allowed to access the data in parallel
        [WriteOnly]
        public NativeArray<float> distances;

        [ReadOnly] public NativeArray<Vector3> centers;
        [ReadOnly] public float time;

        // Delta time must be copied to the job since jobs generally don't have concept of a frame.
        // The main thread waits for the job same frame or next frame, but the job should do work deterministically
        // independent on when the job happens to run on the worker threads.
        public float deltaTime;

        // The code actually running on the job
        public void Execute(int i)
        {
            distances[i] = WaterController.CalculateWave(centers[i]);

        }
    }
    //Mesh for debugging
    private Mesh underWaterMesh;

    //The boats rigidbody
    [SerializeField]
    private Rigidbody boatRB;
    [SerializeField]
    private Transform _t;
    [SerializeField]
    private float scale = 1;
    [SerializeField]
    private bool shouldJob = false;
    //The density of the water the boat is traveling in
    private float rhoWater = 1027f;
    private List<Vector3> centers = new List<Vector3>();
    private List<float> areas = new List<float>();
    private List<Vector3> normals = new List<Vector3>();
    private NativeArray<Vector3> nativeCenters = new NativeArray<Vector3>();
    private NativeArray<float> nativeDistances = new NativeArray<float>();
    public  float[] distances;
    private void CreateArray() { 
        nativeCenters = new NativeArray<Vector3>(centers.Count, Allocator.Persistent); nativeCenters.CopyFrom(centers.ToArray());
        nativeDistances = new NativeArray<float>(centers.Count, Allocator.Persistent); nativeDistances.CopyFrom(areas.ToArray());
        distances = new float[centers.Count];
    }
    private void UpdateCenters()
    {
        for(int i = 0; i < centers.Count; i++) { nativeCenters[i] = _t.TransformPoint(centers[i]); }
    }
    private void DisposeArray() { nativeCenters.Dispose(); nativeDistances.Dispose(); }
    private void OnEnable()
    {
        CreateArray();
    }
    private void OnDestroy()
    {
        DisposeArray();
    }
    void Start()
    {
        //Get the boat's rigidbody
        //boatRB = gameObject.GetComponent<Rigidbody>();
        //_t = transform;
        //Meshes that are below and above the water
        underWaterMesh = _t.GetComponent<MeshFilter>().mesh;
        CalculateMeshData();
    }
    private void CalculateMeshData()
    {
        centers.Clear();
        areas.Clear();
        var tris = underWaterMesh.triangles;
        var verts = underWaterMesh.vertices;
        for (int i = 0; i < tris.Length; i += 3)
        {
            centers.Add(TriangleData.Center(verts[tris[i + 0]], verts[tris[i + 1]], verts[tris[i + 2]]));
            areas.Add(TriangleData.Area(verts[tris[i + 0]], verts[tris[i + 1]], verts[tris[i + 2]]));
            normals.Add(TriangleData.Normal(verts[tris[i + 0]], verts[tris[i + 1]], verts[tris[i + 2]]));
        }
        CreateArray();
    }
    JobHandle handle;
    private void Update()
    {
        if (shouldJob)
        {
            var job = new WaterHeightJob()
            {
                distances = nativeDistances,
                centers = nativeCenters,
                time = Time.time
            };

            // Schedule a parallel-for job. First parameter is how many for-each iterations to perform.
            // The second parameter is the batch size,
            // essentially the no-overhead innerloop that just invokes Execute(i) in a loop.
            // When there is a lot of work in each iteration then a value of 1 can be sensible.
            // When there is very little work values of 32 or 64 can make sense.
            handle = job.Schedule(centers.Count, 32);
            handle.Complete();
            if (handle.IsCompleted) { nativeDistances.CopyTo(distances); }
        }
        else { }
    }
    void FixedUpdate()
    {
        rhoWater = WaterController.rhoWater;

        if (shouldJob) AddUnderWaterForcesJob();
        else AddUnderWaterForces();
    }
    private Vector3 buoyancyForce;
    private Vector3 center;
    //Add all forces that act on the squares below the water
    void AddUnderWaterForces()
    {
        
        float d = 0;
        float g = Physics.gravity.y;
        if (boatRB.velocity.y < -2*g)
        {
            for (int i = 0; i < centers.Count; i++)
            {
                center = _t.TransformPoint(centers[i]);
                d = WaterController.GetWaterHeight(center);
                if (d > 0)
                {
                    //Calculate the buoyancy force
                    //buoyancyForce = BuoyancyForce(rhoWater, center, d, areas[i], Normal(i));
                    buoyancyForce = BuoyancyForce(rhoWater, center, d, areas[i], Normal(i), g);
                    //Add the force to the boat
                    boatRB.AddForceAtPosition(buoyancyForce * Time.fixedDeltaTime * scale, center);
                }
            }
        }
    }
    void AddUnderWaterForcesJob()
    {
        //Debug.Log("Adding Forces");
        float d = 0;
        float g = Physics.gravity.y;
        if (boatRB.velocity.y < -g)
        {
            for (int i = 0; i < centers.Count; i++)
            {
                center = _t.TransformPoint(centers[i]);
                d = distances[i];
                if (center.y > d) d = -1;
                else d = d - center.y;
                if (d > 0)
                {
                    
                    //Calculate the buoyancy force
                    buoyancyForce = BuoyantObject.BuoyancyForce(rhoWater, center, d, areas[i], Normal(i), g);
                    //Debug.Log("adding force" + buoyancyForce);
                    //Add the force to the boat
                    boatRB.AddForceAtPosition(buoyancyForce * Time.fixedDeltaTime * scale, center);
                }
            }
        }
    }
    private Vector3 Normal(int index)
    {

        return TriangleData.Normal(normals[index], _t);
    }
    //The buoyancy force so the boat can float
    private static Vector3 BuoyancyForce(float rho, Vector3 center, float distance, float area, Vector3 normal, float gravity)
    {
        //Buoyancy is a hydrostatic force - it's there even if the water isn't flowing or if the boat stays still

        // F_buoyancy = rho * g * V
        // rho - density of the mediaum you are in
        // g - gravity
        // V - volume of fluid directly above the curved surface 

        // V = z * S * n 
        // z - distance to surface
        // S - surface area
        // n - normal to the surface
        if (distance > 0)
        {
            var buoyancyForce = rho * gravity * distance * area * normal;

            //The vertical component of the hydrostatic forces don't cancel out but the horizontal do
            buoyancyForce.x = 0f;
            buoyancyForce.z = 0f;
            return buoyancyForce;
        }
        else 
        { return Vector3.zero; }
    }
}

