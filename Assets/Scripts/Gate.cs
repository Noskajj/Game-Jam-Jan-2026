using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Gate : MonoBehaviour
{
    [SerializeField]
    private int gateCost= 5000;

    [SerializeField]
    private bool canBeBought = true;

    private InputAction PurchaseGate;
    private bool playerInRange = false;

    [SerializeField]
    private TextMeshPro popupText;


    private void Start()
    {
        if(!canBeBought)
        {
            Destroy(gameObject.GetComponent<Gate>());
        }
        

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
