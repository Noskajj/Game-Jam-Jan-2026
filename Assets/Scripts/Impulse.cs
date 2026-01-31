using UnityEngine;

public class Impulse : MonoBehaviour
{
    public Vector3 force = new Vector3(0, 6, 5);     //Leave X as 0

    private Collider _col;

    private void Awake()
    {
        _col = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Vector3 closest = _col.ClosestPoint(other.transform.position);


        Vector3 direction = other.transform.position - closest;
        direction.y = 0f;
        direction.Normalize();


        float angleRad = Mathf.Atan2(direction.x, direction.z);

        float sin = Mathf.Sin(angleRad);
        float cos = Mathf.Cos(angleRad);

        float x = force.x * cos - force.z * sin;
        float z = -force.x * sin + force.z * cos;

        Vector3 rotatedForce = new Vector3((-1 * x), force.y, z);


        direction.y = 0f;

        Debug.Log("Inside shit");
        if (other.CompareTag("Enemy"))
        {
            PhysicsObjects thing2 = other.GetComponent<PhysicsObjects>();
            thing2.Apply_Force(rotatedForce);
        }
        else if (other.CompareTag("Player"))
        {
            PlayerMovement thing = other.GetComponentInParent<PlayerMovement>();
            thing.Apply_Force(rotatedForce);
        }

    }
}
