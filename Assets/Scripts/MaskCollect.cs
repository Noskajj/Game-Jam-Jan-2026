using UnityEngine;

public class MaskCollect : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            MaskCollected();
        }
    }

    private void MaskCollected()
    {
        MaskManager.Instance.MaskCollected();
        Destroy(gameObject);
    }
}
