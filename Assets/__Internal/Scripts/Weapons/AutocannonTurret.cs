using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutocannonTurret : MonoBehaviour, ITurret
{
    public Rigidbody target { get {return targetVal; } set { targetVal = value; } }
    [SerializeField]
    private Rigidbody targetVal;
    public Shell projectile;
    public int shellCount = 1;
    public List<Transform> muzzles = new List<Transform>();
    public float yAdjust = 10;
    [SerializeField]
    private float fireRate = 30;
    [SerializeField]
    private Transform rotationRoot, elevationRoot;
    [SerializeField]
    private float elevationSpeed, rotationSpeed;
    private Vector3 projectedLocation;
    private float currentTime = 0;
    private int index;
    private bool canFire = false;

    public void Fire()
    {
        if (canFire)
        {
            index++;
            index %= muzzles.Count;
            for (int i = 0; i < shellCount; i++) { Instantiate(projectile, muzzles[index].position, muzzles[index].rotation); }
            currentTime = 0;
            canFire = false;
        }
    }

    public void Track()
    {
        Quaternion q = Quaternion.RotateTowards(rotationRoot.rotation, CalculateRotation(), rotationSpeed * Time.deltaTime);
        
        
        rotationRoot.rotation = q;// Quaternion.Euler(v);
        Vector3 v = rotationRoot.localEulerAngles;
        v.x = 0;
        v.z = 0;
        rotationRoot.localRotation = Quaternion.Euler(v);
        q = Quaternion.RotateTowards(elevationRoot.rotation, CalculateElevation(), elevationSpeed * Time.deltaTime);
        elevationRoot.rotation = q;
        v = elevationRoot.localEulerAngles;
        v.y = 0;
        v.z = 0;
        elevationRoot.localRotation = Quaternion.Euler(v);
    }
    public Quaternion CalculateRotation() 
    {
        Quaternion rotation;
        Quaternion lookRotation = Quaternion.LookRotation(projectedLocation - rotationRoot.position, transform.up);
        Vector3 euler = lookRotation.eulerAngles;
        //euler.x = 0; euler.z = 0;
        rotation = Quaternion.Euler(euler);
        return rotation;
    }
    public Quaternion CalculateElevation()
    {
        Quaternion rotation;
        Quaternion lookRotation = Quaternion.LookRotation(projectedLocation - elevationRoot.position, elevationRoot.up);
        Vector3 euler = lookRotation.eulerAngles;
        //euler.y = 0; euler.z = 0;
        rotation = Quaternion.Euler(euler);
        return rotation;
    }
    private Vector3 PredictLocation()
    {
        if (target) { 
            projectedLocation = (target.velocity.magnitude / projectile.velocity) * target.velocity + target.position;
            projectedLocation += (Vector3.up * -Physics.gravity.y * Vector3.Distance(projectedLocation, transform.position) / projectile.velocity);
            projectedLocation.y += yAdjust;
            return projectedLocation; }
        else return transform.position + 100 * transform.forward;
    }
    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        if(currentTime > 1/fireRate) { canFire = true; }
        else { canFire = false; }
        if(target != null) { PredictLocation(); Track(); }
        if(target == null) { target = FindObjectOfType<Rigidbody>(); }
        //if(Input.GetMouseButton(0)) { Fire(); }
    }
}
