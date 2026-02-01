using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShopKeep : MonoBehaviour
{

    [SerializeField]
    private TextMeshPro popupText;

    //Some sort of detection
    private InputAction OpenShop;

    private bool playerInRange = false;

    private void Start()
    {
        OpenShop = InputSystem.actions.FindAction("Interact");
        OpenShop.started += OpenShopUi;


        popupText.text = $" 'E' ";
    }

    private void OpenShopUi(InputAction.CallbackContext context)
    {
        if (playerInRange)
        {
            //open shop
            UpgradeManager.Instance.OpenShopUi();
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
