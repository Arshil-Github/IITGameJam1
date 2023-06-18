using UnityEngine;

public class PointinTime
{
    public Vector3 position;
    public Quaternion rotation;
    public bool isActive;
    public int health;
    public PointinTime(Vector3 _position, Quaternion _rotation, bool _isActive, int _health)
    {
        position = _position;
        rotation = _rotation;
        isActive = _isActive;
        health = _health;
    }
}
