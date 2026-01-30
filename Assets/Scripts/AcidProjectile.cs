using UnityEngine;

public class AcidProjectile : MonoBehaviour
{
    public float speed = 10f;
    public float lifeTime = 10f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject,lifeTime);
    }

    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

}
