using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDestructible
{
    public void OnDamage(int damage);
    public void OnDestroyed();
}
