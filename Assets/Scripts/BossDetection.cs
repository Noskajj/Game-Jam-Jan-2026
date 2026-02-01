using UnityEngine;

public class BossDetection : MonoBehaviour
{
    [SerializeField]
    private GameObject boss;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            boss.SetActive(true);
        }
    }
}
