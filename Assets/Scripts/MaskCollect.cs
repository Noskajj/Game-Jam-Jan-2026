using UnityEngine;

public class MaskCollect : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        //TODO: Edit this to use input to pickup
        if(other.CompareTag("Player"))
        {
            MaskCollected();
        }
    }

    private void MaskCollected()
    {
        MaskManager.Instance.MaskCollected();
        //Play maskCollected sound
        MainMenuAudioManager.instance.PlayOneShot(FMODEvents.instance.maskCollected, this.transform.position);
        Destroy(gameObject);
    }

}
