using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour, IProjectile
{
    public float velocity;
    public float lifetime;
    public Rigidbody target;
    public float rotationSpeed = 5;
    public int damage = 500;
    public LayerMask mask;
    public MissileGuidance guidance;
    private Vector3 projectedLocation;
    
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(target.name);
    }
    public void Track()
    {
        Quaternion q = Quaternion.RotateTowards(transform.rotation, CalculateRotation(), rotationSpeed * Time.deltaTime);
        transform.rotation = q;
    }
    public Vector3 PredictLocation()
    {
        return projectedLocation = (target.velocity.magnitude / velocity) * target.velocity + target.position;
    }
    public Quaternion CalculateRotation()
    {
        Quaternion rotation;
        Quaternion lookRotation = Quaternion.LookRotation(projectedLocation - transform.position, transform.up);
        Vector3 euler = lookRotation.eulerAngles;
        //euler.x = 0; euler.z = 0;
        rotation = Quaternion.Euler(euler);
        return rotation;
    }
    // Update is called once per frame
    void Update()
    {
        lifetime -= Time.deltaTime;
        if (lifetime <= 0)
        {
            Destroy(gameObject);
        }
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, maxDistance: velocity * Time.deltaTime, layerMask:mask))
        {
            //Debug.Log(hit.point);
            Hit(hit.transform);
        }
        if (target) 
        { 
            if(guidance != null) 
            { 
                projectedLocation = guidance.PredictLocation();
                Track();
            }
            
        }

        
        Motor();
    }
    public void Motor()
    {
        transform.Translate(Vector3.forward * velocity * Time.deltaTime);
    }
    public void Hit(Transform other)
    {
        Debug.Log("Hit:" + other.name);
        var t = other.GetComponentInParent<IDestructible>();
        if (t != null) { t.OnDamage(damage); }
        Destroy(gameObject);
    }
}
