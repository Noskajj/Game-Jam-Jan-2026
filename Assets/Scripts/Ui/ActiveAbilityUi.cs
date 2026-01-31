using System.Collections;
using System.Threading;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ActiveAbilityUi : MonoBehaviour
{
    [SerializeField]
    private int ability = 0;
    [SerializeField]
    private TextMeshProUGUI CDTimer, gunAmmo;
    [SerializeField]
    private Image CDOverlay, bgImage;

    private void Start()
    {
        CDTimer.text = "";
        CDOverlay.fillAmount = 0f;

        switch (ability)
        {
            case 0:
                MaskManager.Instance.Mask1Invoked += StartCooldown;
                break;
            case 1:
                MaskManager.Instance.Mask2Invoked += StartCooldown;
                break;
            case 2:
                MaskManager.Instance.Mask3Invoked += StartCooldown;
                break;
            case 3:
                PlayerManager.Instance.SwordInvoked += StartCooldown;
                break;
            case 4:
                PlayerManager.Instance.GunInvoked += StartCooldown;
                break;
        }
    }

    private void Update()
    {
        if(ability == 4)
        {
            gunAmmo.text = PlayerStats.CurrentAmmo.ToString();
        }
    }

    public void StartCooldown((float cooldown, float active) values)
    {
        float cooldown = values.cooldown;
        float active = values.active;

        
        CDTimer.text = cooldown.ToString();
        
        StartCoroutine(ActiveTimer(active, cooldown));
    }

    private IEnumerator ActiveTimer(float time, float cdTime)
    {
        float timer = 0f;
        CDOverlay.fillAmount = 0f;

        while (timer < time)
        {
            timer += Time.deltaTime;
            float remaining = Mathf.Max(0f, time - timer);
            CDTimer.text = remaining.ToString("F1");

            yield return null;
        }

        StartCoroutine(CooldownAnimation(cdTime));
    }

    private IEnumerator CooldownAnimation(float cooldown)
    {
        CDOverlay.fillAmount = 1f;
        float timer = 0f;

        while (timer < cooldown)
        {
            timer += Time.deltaTime;
            CDOverlay.fillAmount = 1f - (timer / cooldown);
            //Debug.Log($"Overlay fill amount is {CDOverlay.fillAmount}");
            float remaining = Mathf.Max(0f, cooldown - timer);
            CDTimer.text =  remaining.ToString("F1");

            yield return null;
        }

        CDOverlay.fillAmount = 0f;
        CDTimer.text = "";
        
    }


}
