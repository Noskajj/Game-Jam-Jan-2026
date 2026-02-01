using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Gate : MonoBehaviour
{
    [SerializeField]
    private int gateCost= 5000;

    private InputAction PurchaseGate;
    private bool playerInRange = false;

    [SerializeField]
    private TextMeshPro popupText;

    private void Start()
    {
        PurchaseGate = InputSystem.actions.FindAction("Interact");
        PurchaseGate.started += BuyGate;

        popupText.text = $" 'E' \n" +
            $"Cost: {gateCost}";
    }

    private void BuyGate(InputAction.CallbackContext context)
    {
        if(playerInRange)
        {
            if (CheckAffordGate())
            {
                PlayerStats.SpendSouls(gateCost);
                Destroy(gameObject);
            }
        }
    }

    private bool CheckAffordGate()
    {
        if(gateCost <= PlayerStats.Souls)
        {
            return true;
        }
        return false;
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
