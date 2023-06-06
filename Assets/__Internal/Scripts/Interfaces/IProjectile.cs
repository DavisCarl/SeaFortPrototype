using UnityEngine;

public interface IProjectile
{
    public void Hit(Transform target);
    public void Motor();
}