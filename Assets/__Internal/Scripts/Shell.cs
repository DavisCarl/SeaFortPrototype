using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell : MonoBehaviour, IProjectile
{
    public float velocity;
    public float lifetime;
    public Vector2 dispersion;
    public int damage = 10;
    private Rigidbody rb;
    private void Start()
    {
        transform.Rotate(Disperse());
        rb = transform.GetComponent<Rigidbody>();
        rb.velocity = velocity * transform.forward;
    }
    private Vector3 Disperse()
    {
        return new Vector3(Random.Range(-dispersion.x, dispersion.x), 0, Random.Range(-dispersion.y, dispersion.y));
    }
    private void Update()
    {
        lifetime-= Time.deltaTime;
        if(lifetime <= 0)
        {
            Destroy(gameObject);
        }
        // RaycastHit hit; 
        //if (Physics.Raycast(transform.position, transform.forward, out hit, maxDistance: velocity * Time.deltaTime))
        //{
        //    Debug.Log(hit.point);
        //    Hit();
        //}
        //Motor();
    }
    void OnCollisionEnter(Collision collision)
    {
        Hit(collision.transform);
    }
    public void Motor()
    {
        transform.Translate(Vector3.forward*velocity*Time.deltaTime);
    }
    public void Hit(Transform other)
    {
        //Debug.Log();
        var t = other.GetComponentInParent<IDestructible>();
        if (t != null) { t.OnDamage(damage); }
        else { Debug.Log("Hit:" + other.name + " No Destructible Found"); }
        Destroy(gameObject);
    }
}
