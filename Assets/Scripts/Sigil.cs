using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Sigil : MonoBehaviour
{
    [SerializeField]
    private SigilType sigilType;

    [SerializeField]
    private TextMeshPro popupText;

    //Some sort of detection
    private InputAction PurchaseSigil;

    private bool playerInRange = false;

    private void Start()
    {
        PurchaseSigil = InputSystem.actions.FindAction("Interact");
        PurchaseSigil.started += BuySigil;


        popupText.text = $" 'E' \n" +
            $"Pact of {sigilType} \n" +
            $"Cost: {SigilManager.Instance.SigilCost(sigilType)}";
    }

    private void BuySigil(InputAction.CallbackContext context)
    {
        if(playerInRange)
        {
            if (SigilManager.Instance.CanPurchaseSigil(sigilType))
            {
                SigilManager.Instance.PurchaseSigil(sigilType);
                //Play magicalInfusion
                MainMenuAudioManager.instance.PlayOneShot(FMODEvents.instance.magicalInfusion, this.transform.position);
            }
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            popupText.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            popupText.enabled = false;
        }
    }
}
