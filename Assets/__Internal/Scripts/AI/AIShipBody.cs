using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIShipBody : MonoBehaviour,IDestructible
{
    public void OnDamage(int damage)
    {
        health -= damage;
    }

    public void OnDestroyed()
    {
        var t  = Instantiate(destructionObject) ;
        t.SetPositionAndRotation(transform.position, transform.rotation);
        Destroy(gameObject);
    }
    public int health = 2500;
    public Transform destructionObject;

    // Update is called once per frame
    void Update()
    {
        if(health < 0) { OnDestroyed(); }
    }
}
