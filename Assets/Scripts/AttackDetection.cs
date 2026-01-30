using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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

    private void Start()
    {
        sphereCollider = GetComponent<SphereCollider>();
    }

    private void Update()
    {
        MoveColliderWithMouse();
    }

    #region FollowMouse
    private float orbitDist = 1.5f;

    private void MoveColliderWithMouse()
    {
        //Get screen center 
        Vector2 screenCenter = new Vector2(Screen.width/2, Screen.height/2);

        //Get mouse offset
        Vector2 mouseOffset = Mouse.current.position.ReadValue() - screenCenter;

        if (mouseOffset.sqrMagnitude < 0.01f)
            return;

        //Convert to normalized direction
        Vector3 direction = new Vector3(mouseOffset.x, 0f, mouseOffset.y).normalized;

        //move collider around player
        transform.position = transform.parent.position + direction * orbitDist;
    }
    #endregion

    #region Attacking
    private SphereCollider sphereCollider;

    private readonly HashSet<Collider> enemiesInRange = new();

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            enemiesInRange.Add(other);
            Debug.Log("We Got enmies");
        }
            
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemiesInRange.Remove(other);
            Debug.Log("no enmies");
        }
    }

    public void Attack()
    {
        foreach(var enemy in enemiesInRange)
        {
            Debug.Log("attak enmies");
            enemy.GetComponent<EnemyClass>().TakeDamage((int)PlayerStats.meleeDamage);
        }

    }

    #endregion
}
