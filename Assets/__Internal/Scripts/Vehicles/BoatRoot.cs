using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatRoot : MonoBehaviour
{
    public Transform hullTransform { get; private set; }
    public Transform buoyancyTransform { get; private set; }
    [SerializeField] 
    private Transform _hullTransform, _buoyancyTransform;
    private void Start()
    {
        hullTransform = Instantiate(_hullTransform);
        buoyancyTransform = Instantiate(_buoyancyTransform);
    }
}
