using TMPro;
using UnityEngine;
using Project.Scripts.Dennis.Game;

public class TimerUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private RectTransform timerFill;
    [SerializeField] private TMP_Text timerValue;

    [Header("Timer")]
    [SerializeField] private float totalTime = 600f;

    [Header("Fill Positions")]
    [SerializeField] private float fullPosX = 0f;
    [SerializeField] private float emptyPosX = -377.77f;

    [Header("Game Manager")]
    [SerializeField] private GameManager gameManager;

    private float remainingTime;

    private void Start()
    {
        if (!gameManager)
        {
            gameManager = FindAnyObjectByType<GameManager>();
        }

        remainingTime = totalTime;
        UpdateTimerUI();
    }

    private void Update()
    {
        if (remainingTime <= 0f)
        {
            if (gameManager && gameManager.State == GameManager.GameState.Playing)
            {
                gameManager.Lose();
            }

            return;
        }

        remainingTime -= Time.deltaTime;
        remainingTime = Mathf.Max(remainingTime, 0f);

        UpdateTimerUI();
    }

    private void UpdateTimerUI()
    {
        float timePercent = remainingTime / totalTime;
        float newPosX = Mathf.Lerp(emptyPosX, fullPosX, timePercent);

        Vector2 position = timerFill.anchoredPosition;
        position.x = newPosX;
        timerFill.anchoredPosition = position;

        int minutes = Mathf.FloorToInt(remainingTime / 60f);
        int seconds = Mathf.FloorToInt(remainingTime % 60f);

        timerValue.text = $"{minutes:00}:{seconds:00}";
    }
}