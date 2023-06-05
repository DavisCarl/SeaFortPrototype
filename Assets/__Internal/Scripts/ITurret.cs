

using UnityEngine;

public interface ITurret 
{
    public Rigidbody target { get; set; }
    public void Fire();
    public void Track();
}
