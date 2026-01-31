using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AttackDetection : MonoBehaviour
{
    public Vector3 meleeForce = new Vector3(0, 6, 35);  //Melee strength

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
        List<Collider> deadEnemys = null;

        foreach (var enemyObj in enemiesInRange)
        {

            var enemy = enemyObj.GetComponentInParent<EnemyClass>();

            if (enemy == null) continue;

            Debug.Log("Attack Enemy Melee");

            bool isDead = enemy.TakeDamage((int)PlayerStats.MeleeDamage);

            //Stupid physics shit start:

            var enemyPush = enemyObj.GetComponentInParent<PhysicsObjects>();

            if (enemyPush != null)
            {
                enemyPush.Apply_Force(Impulse_Vector(enemyObj)); //TODO change
            }

            //Stupid physics shit end;

            if (isDead) 
            {
                deadEnemys ??= new List<Collider>();
                deadEnemys.Add(enemyObj);

            }



        }
        //FREE MEMORY CODE DO NOT DELETE
        if (deadEnemys != null)
        {
            for (int i = 0; i < deadEnemys.Count; i++)
            {
                enemiesInRange.Remove(deadEnemys[i]);
            }
        }

    }

    private Vector3 Impulse_Vector(Collider enemy)
    {
        Vector3 direction = enemy.transform.position - transform.position;

        direction.y = 0f;         //We dont care about y angle

        direction.Normalize();

        float angleRad = Mathf.Atan2(direction.x, direction.z);

        float x = -1 * meleeForce.x * Mathf.Cos(angleRad) - meleeForce.z * Mathf.Sin(angleRad);
        float z = -1* meleeForce.x * Mathf.Sin(angleRad) - meleeForce.z * Mathf.Cos(angleRad);

        Vector3 rotatedForce = new Vector3((-1 * x), meleeForce.y, z);
        direction.y = 0f;


        return rotatedForce;
    }

    #endregion
}
