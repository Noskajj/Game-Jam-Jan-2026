using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class AttackDetection : MonoBehaviour
{
    public static AttackDetection Instance { get; private set;}
    
    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }

        Instance = this;
    }

    private SphereCollider sphereCollider;

    private readonly HashSet<Collider> enemiesInRange = new();

    private void Start()
    {
        sphereCollider = GetComponent<SphereCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
            enemiesInRange.Add(other);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
            enemiesInRange.Remove(other);
    }

    public void Attack()
    {
        foreach(var enemy in enemiesInRange)
        {
            enemy.GetComponent<EnemyClass>().TakeDamage((int)PlayerStats.meleeDamage);
        }

    }
}
