using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class UiHandler : MonoBehaviour
{
    [Header("Souls")]
    [SerializeField]
    private TextMeshProUGUI soulsText;

    [Header("Hp")]
    [SerializeField]
    private Image hpImage;
    [SerializeField]
    private float hpAnimateTime = 1f;
    private Coroutine hpAnimateCoroutine;

    [Header("Stamina")]
    [SerializeField]
    private Image stamImage;
    [SerializeField]
    private float stamAnimateTime = 1f;
    private Coroutine stamAnimateCoroutine;

    [Header("Waves")]
    [SerializeField]
    private TextMeshProUGUI waveTxt;
    [SerializeField]
    private CanvasGroup waveCg;
    private float waveAnimateTime = 1f;

    private void Awake()
    {
        PlayerStats.maxHealthChanged += UpdateHpUi;
        PlayerStats.healthUpdated += UpdateHpUi;
        PlayerStats.stamUpdated += UpdateStamUi;
        PlayerStats.maxStamChanged += UpdateStamUi;
        PlayerStats.soulsUpdated += UpdateSoulsUi;
        EnemySpawner.waveUpdated += UpdateWaveUi;
    }

    private void OnDisable()
    {
        PlayerStats.maxHealthChanged -= UpdateHpUi;
        PlayerStats.healthUpdated -= UpdateHpUi;
        PlayerStats.stamUpdated -= UpdateStamUi;
        PlayerStats.maxStamChanged -= UpdateStamUi;
        PlayerStats.soulsUpdated -= UpdateSoulsUi;
        EnemySpawner.waveUpdated -= UpdateWaveUi;
    }

    private void UpdateSoulsUi()
    {
        soulsText.text = $"{PlayerStats.Souls}";
    }

    private void UpdateHpUi()
    {
        if(hpAnimateCoroutine != null)
            StopCoroutine(hpAnimateCoroutine);

        hpAnimateCoroutine = StartCoroutine(AnimateHpBar(PlayerStats.Health, PlayerStats.MaxHealth, hpAnimateTime));
    }

    private IEnumerator AnimateHpBar(int currentHp, int totalHp, float animateTime)
    {
        float timer = 0f;
        float startFill = hpImage.fillAmount;
        float endFill = Mathf.Clamp01((float)currentHp / totalHp);

            while (timer < animateTime)
            {
                timer += Time.deltaTime;
                float t = timer / animateTime;

                hpImage.fillAmount = Mathf.Lerp(startFill, endFill, t);
                yield return null;
            }
            //Makes the bar be filled by the correct %, and also clamps it between 0 and 1
            hpImage.fillAmount = endFill;
            yield return null;
    }

    private void UpdateStamUi()
    {
        if (stamAnimateCoroutine != null)
            StopCoroutine(stamAnimateCoroutine);

        stamAnimateCoroutine = StartCoroutine(AnimateStamBar(PlayerStats.CurrentStamina, PlayerStats.MaxStamina, stamAnimateTime));
    }

    private IEnumerator AnimateStamBar(int currentStam, int totalStam, float animateTime)
    {
        float timer = 0f;
        float startFill = stamImage.fillAmount;
        float endFill = Mathf.Clamp01((float)currentStam / totalStam);

        while (timer < animateTime)
        {
            timer += Time.deltaTime;
            float t = timer / animateTime;

            stamImage.fillAmount = Mathf.Lerp(startFill, endFill, t);
            yield return null;
        }
        //Makes the bar be filled by the correct %, and also clamps it between 0 and 1
        stamImage.fillAmount = endFill;
        yield return null;
    }

    private void UpdateWaveUi()
    {
        waveTxt.text = $"Wave: {EnemySpawner.Instance.WaveNumber}";

        StartCoroutine(WaveFadeIn());
    }

    private IEnumerator WaveFadeIn()
    {
        float timer = 0f;

        while (timer < waveAnimateTime)
        {
            timer += Time.deltaTime;
            float t = timer / waveAnimateTime;

            waveCg.alpha = Mathf.Lerp(0, 1, t);
            yield return null;
        }
        //Makes the bar be filled by the correct %, and also clamps it between 0 and 1
        waveCg.alpha = 1;
        yield return new WaitForSeconds(2f);

        StartCoroutine(WaveFadeOut());
    }

    private IEnumerator WaveFadeOut()
    {
        float timer = 0f;

        while (timer < waveAnimateTime)
        {
            timer += Time.deltaTime;
            float t = timer / waveAnimateTime;

            waveCg.alpha = Mathf.Lerp(1, 0, t);
            yield return null;
        }
        //Makes the bar be filled by the correct %, and also clamps it between 0 and 1
        waveCg.alpha = 0;
        yield return null;
    }
}
