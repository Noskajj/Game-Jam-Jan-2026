using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.GraphicsBuffer;

public class AttackDetection : MonoBehaviour
{
    public Vector3 meleeForce = new Vector3(0, 10, 35);  //Melee strength

    public static AttackDetection Instance { get; private set;}

    //player animation
    public Animator playerAnimator;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }

        Instance = this;
    }   

    #region Attacking

    public void Attack()
    {
        Vector3 playerPos = transform.position;

        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

        Plane groundPlane = new Plane(Vector3.up, playerPos);

        Vector3 mouseWorldPos = playerPos;

        if(groundPlane.Raycast(ray, out float hitDist))
        {
            mouseWorldPos = ray.GetPoint(hitDist);
        }

        Vector3 direction = (mouseWorldPos - playerPos).normalized;

        //This is the same idea as a radius for a sphere
        Vector3 halfExtents = Vector3.one;

        Vector3 boxCenter = playerPos + direction * halfExtents.z;

        Collider[] hits = Physics.OverlapBox(
                boxCenter,
                halfExtents
            );

        foreach (var hit in hits)
        {
            if(hit.TryGetComponent<EnemyClass>(out EnemyClass enemy))
            {
                Debug.Log("Attack Enemy Melee");

                bool isDead = enemy.TakeDamage((int)PlayerStats.MeleeDamage);
            }
        }
        //Elliots stuff, idk how it works

        /*foreach (var enemyObj in enemiesInRange)
        {

            var enemy = enemyObj.GetComponentInParent<EnemyClass>();

            if (enemy == null) continue;

            

            //Stupid physics shit start:

            var enemyPush = enemyObj.GetComponentInParent<PhysicsObjects>();

            if (enemyPush != null)
            {
                enemyPush.Apply_Force(Impulse_Vector(enemyObj)); //TODO change
            }


        }*/
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
