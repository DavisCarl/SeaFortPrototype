using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerShipFireControl : MonoBehaviour
{
    public List<Transform> missileTurretTransforms = new List<Transform>();
    public List<Transform> autocannonTurretTransforms = new List<Transform>();
    private List<MissileTurret> missileTurrets = new List<MissileTurret>();
    private List<AutocannonTurret>  autocannonTurrets = new List<AutocannonTurret>();
    public Rigidbody target;
    public Transform targetIndicator;
    private void UpdateAutocannons() 
    {
        autocannonTurrets.Clear();
        foreach(Transform t in autocannonTurretTransforms) { var ac = t.GetComponentInChildren<AutocannonTurret>(); if (ac) { autocannonTurrets.Add(ac); } }
    }
    private void UpdateMissileTurrets()
    {
        missileTurrets.Clear();
        foreach (Transform t in missileTurretTransforms) { var mi = t.GetComponentInChildren<MissileTurret>(); if (mi) { missileTurrets.Add(mi); } }
    }
    private void UpdateIndicator()
    {
        if (target)
        {
            foreach(Renderer r in targetIndicator.GetComponentsInChildren<Renderer>()) {  r.enabled = true; }
        }
        else
        { 
            foreach (Renderer r in targetIndicator.GetComponentsInChildren<Renderer>()) { r.enabled = false; } 
        }
    }
    private void TrackIndicator()
    {
        if(target) { 
            targetIndicator.position = target.transform.position;
            if (Vector3.Distance(target.position, transform.position) > 100)
            {
                targetIndicator.localScale = Vector3.one * (Vector3.Distance(target.position, transform.position) / 10);
            }
            else { targetIndicator.localScale = Vector3.one * 10; }
            targetIndicator.LookAt(transform);
        }
    }
    public void FireMissiles()
    {
        foreach (var t in missileTurrets)
        {
            t.Fire();
        }
    }
    public void FireAutocannons()
    {
        foreach (var t in autocannonTurrets)
        {
            t.Fire();
        }
    }
    private void UpdateTargets()
    {
        foreach(AutocannonTurret t in autocannonTurrets) { t.target = target; }
        foreach (MissileTurret t in missileTurrets) { t.target = target; }
        UpdateIndicator();
    }
    private void Start()
    {
        Init();
    }
    private void Update()
    {
        TrackIndicator();
    }
    public void SetTarget(Rigidbody t)
    {
        target = t;
        UpdateTargets();
    }
    public void Register()
    {
        PlayerInput.shipFireControl = this;
    }
    private void Init()
    {
        UpdateAutocannons();
        UpdateMissileTurrets();
        Register();
    }
}
