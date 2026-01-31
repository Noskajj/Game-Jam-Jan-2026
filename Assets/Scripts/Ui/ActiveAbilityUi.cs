using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActiveAbilityUi : MonoBehaviour
{
    [SerializeField]
    private int ability = 0;
    [SerializeField]
    private TextMeshProUGUI CDTimer;
    [SerializeField]
    private Image CDOverlay, bgImage;

    private void Start()
    {
        switch(ability)
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
        }
    }

    public void StartCooldown(float cooldown)
    {
        CDOverlay.fillAmount = 1f;
        CDTimer.text = cooldown.ToString();
        
        StartCoroutine(CooldownAnimation(cooldown));
    }

    private IEnumerator CooldownAnimation(float cooldown)
    {
        float timer = 0f;

        while (timer < cooldown)
        {
            timer += Time.deltaTime;
            CDOverlay.fillAmount = 1f - (timer / cooldown);
            float remaining = Mathf.Max(0f, cooldown - timer);
            CDTimer.text =  remaining.ToString("F1");

            yield return null;
        }

        CDOverlay.fillAmount = 0f;
        CDTimer.text = "";
        
    }
}
