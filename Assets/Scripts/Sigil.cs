using UnityEngine;
using UnityEngine.InputSystem;

public class Sigil : MonoBehaviour
{
    [SerializeField]
    private SigilType sigilType = SigilType.Blood;

    //Some sort of detection
    private InputAction PurchaseSigil;

    private bool playerInRange = false;

    private void Start()
    {
        PurchaseSigil = InputSystem.actions.FindAction("Interact");
        PurchaseSigil.started += BuySigil;
    }

    private void BuySigil(InputAction.CallbackContext context)
    {
        if(playerInRange)
        {
            if (SigilManager.Instance.CanPurchaseSigil(sigilType))
            {
                SigilManager.Instance.PurchaseSigil(sigilType);
            }
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
