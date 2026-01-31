using UnityEngine;

public class EnemyMelee : MonoBehaviour
{

    private MeleeClass meleeClass;
    private void Awake()
    {
        meleeClass = GetComponentInParent<MeleeClass>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            meleeClass.PlayerEnteredMeleeRange();
            Debug.Log("Melee hitbox colliding with player");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            meleeClass.PlayerExitMeleeRange();
        }
    }
}
