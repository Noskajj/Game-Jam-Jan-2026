using TMPro;
using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    public static GameOverManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }

        Instance = this;

    }

    [SerializeField]
    private GameObject gameOverPanel;


    [SerializeField]
    private TextMeshProUGUI gameOverTxt;

    public void OpenPanel()
    {
        Time.timeScale = 0;
        gameOverPanel.SetActive(true);
        gameOverTxt.text = $"You survived until Round {EnemySpawner.Instance.WaveNumber}." +
            $"You managed to collect {MaskManager.Instance.AllMasks} out of {MaskManager.Instance.MasksCollected} Masks" +
            $" and had {PlayerStats.Souls} souls at the end.";
    }

    public void ClosePanel()
    {
        Time.timeScale = 1;
        gameOverPanel.SetActive(false); 
    }


}
