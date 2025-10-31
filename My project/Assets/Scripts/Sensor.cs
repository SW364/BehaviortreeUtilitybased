// Sensor.cs
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[RequireComponent(typeof(SphereCollider))]
public class Sensor : MonoBehaviour
{
    public float detectionRadius = 15f;
    public List<GameObject> detectedEnemies = new List<GameObject>();
    private SphereCollider _collider;

    void Awake()
    {
        _collider = GetComponent<SphereCollider>();
        _collider.isTrigger = true;
        _collider.radius = detectionRadius;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (!detectedEnemies.Contains(other.gameObject))
            {
                detectedEnemies.Add(other.gameObject);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (detectedEnemies.Contains(other.gameObject))
            {
                detectedEnemies.Remove(other.gameObject);
            }
        }
    }
}