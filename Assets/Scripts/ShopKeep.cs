using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShopKeep : MonoBehaviour
{

    [SerializeField]
    private TextMeshPro popupText;

    //Defines Variable OpenShop which can be set to input keys
    private InputAction OpenShop;

    private bool playerInRange = false;
    private bool shopUIisOpen = false;


    
    private void Start()
    {
        OpenShop = InputSystem.actions.FindAction("Interact"); //Set OpenShop to monitor the interact key
        
        OpenShop.Enable(); //Turn on monitoring
        
        OpenShop.performed += OpenShopUi; //Run Test Every time OpenShop monitors an interaction with interact key
        
        
        popupText.text = $" 'E' ";
    }




    private void OpenShopUi(InputAction.CallbackContext context)
    {
        
        if (playerInRange && PlayerStats.Health > 0 && shopUIisOpen == false)
        {

            //open shop

            UpgradeManager.Instance.OpenShopUi();
            shopUIisOpen = true;

        }
        else if (playerInRange && PlayerStats.Health > 0 && shopUIisOpen == true)
        { 

            //close shop
            shopUIisOpen = false;
            UpgradeManager.Instance.CloseShopUi();


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
