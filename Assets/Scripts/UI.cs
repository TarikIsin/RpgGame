using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class UI : MonoBehaviour
{
    public static UI Instance;
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI killCountText;
    [SerializeField] private TextMeshProUGUI characterHealthText;
    [SerializeField] private TextMeshProUGUI girlHealthText;

    private int killCount;
    private float startTime; 

    private void Awake()
    {
        Instance = this;
        Time.timeScale = 1.0f;
        startTime = Time.time; 
    }

    private void Update()
    {
        float elapsed = Time.time - startTime;
        timerText.text = elapsed.ToString("F2") + "s";
    }

    public void UpdateCharacterHealth(int current)
    {
        characterHealthText.text = current.ToString();
    }

    public void UpdateGirlHealth(int current)
    {
        girlHealthText.text = current.ToString();
    }

    public void EnableGameOverUI()
    {
        StartCoroutine(SlowDown());
        gameOverUI.SetActive(true);
    }

    private IEnumerator SlowDown()
    {
        float duration = 2;

        while (Time.timeScale > 0f)
        {
            Time.timeScale = Mathf.Max(Time.timeScale - Time.unscaledDeltaTime / duration, 0f);
            yield return null;
        }
    }

    public void RestartLevel()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(sceneIndex);
    }

    public void AddKillCount()
    {
        killCount++;
        killCountText.text = killCount.ToString();
    }
}