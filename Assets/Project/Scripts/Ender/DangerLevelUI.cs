using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Project.Scripts.Dennis.Game;

public class DangerLevelUI : MonoBehaviour
{
    [Header("Color Fill Images")]
    [SerializeField] private Image dangerLevelOrange;
    [SerializeField] private Image dangerLevelRed;
    [SerializeField] private Image dangerLevelPurple;

    [Header("Skeleton Images")]
    [SerializeField] private Image dangerSkeletonLevel1;
    [SerializeField] private Image dangerSkeletonLevel2;
    [SerializeField] private Image dangerSkeletonLevel3;
    [SerializeField] private Image dangerSkeletonLevel4;

    [Header("Game Manager")]
    [SerializeField] private GameManager gameManager;

    [Header("Shake Settings")]
    [SerializeField] private float shakeStartPercent = 0.97f;
    [SerializeField] private float minShakeStrength = 0.05f;
    [SerializeField] private float maxShakeStrength = 2f;
    [SerializeField] private float minShakeSpeed = 15f;
    [SerializeField] private float maxShakeSpeed = 40f;

    [Header("Level Up Pop")]
    [SerializeField] private float popScale = 1.4f;
    [SerializeField] private float popDuration = 0.35f;

    private int currentStage = 0;
    private float currentFill = 0f;

    private RectTransform rectTransform;
    private Vector2 startPosition;
    private Vector3 startScale;

    private bool isPopping = false;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        startPosition = rectTransform.anchoredPosition;
        startScale = rectTransform.localScale;

        if (!gameManager)
        {
            gameManager = FindAnyObjectByType<GameManager>();
        }
    }

    private void OnEnable()
    {
        if (gameManager)
        {
            gameManager.OnAlarmChanged += HandleAlarmChanged;
        }
    }

    private void Start()
    {
        dangerLevelOrange.fillAmount = 0f;
        dangerLevelRed.fillAmount = 0f;
        dangerLevelPurple.fillAmount = 0f;

        SetSkeleton(1);

        if (gameManager)
        {
            HandleAlarmChanged(gameManager.Alarm, gameManager.MaxAlarm);
        }
    }

    private void OnDisable()
    {
        if (gameManager)
        {
            gameManager.OnAlarmChanged -= HandleAlarmChanged;
        }
    }

    private void Update()
    {
        UpdateShake();
    }

    private void HandleAlarmChanged(float currentAlarm, float maxAlarm)
    {
        if (maxAlarm <= 0f)
            return;

        float alarmPercent = Mathf.Clamp01(currentAlarm / maxAlarm);

        UpdateDangerDisplay(alarmPercent);
    }

    private void UpdateDangerDisplay(float alarmPercent)
    {
        int previousStage = currentStage;

        float scaledProgress = alarmPercent * 3f;

        if (alarmPercent >= 1f)
        {
            currentStage = 3;
            currentFill = 1f;
        }
        
        else
        {
            currentStage = Mathf.FloorToInt(scaledProgress);
            currentFill = scaledProgress - currentStage;
        }

        dangerLevelOrange.fillAmount = 0f;
        dangerLevelRed.fillAmount = 0f;
        dangerLevelPurple.fillAmount = 0f;

        switch (currentStage)
        {
            case 0: 
                dangerLevelOrange.fillAmount = currentFill;
                break;

            case 1: 
                dangerLevelOrange.fillAmount = 1f;
                dangerLevelRed.fillAmount = currentFill;
                break;

            case 2: 
                dangerLevelOrange.fillAmount = 1f;
                dangerLevelRed.fillAmount = 1f;
                dangerLevelPurple.fillAmount = currentFill;
                break;

            case 3:
                dangerLevelOrange.fillAmount = 1f;
                dangerLevelRed.fillAmount = 1f;
                dangerLevelPurple.fillAmount = 1f;
                break;
        }

        SetSkeleton(currentStage + 1);

        if (currentStage > previousStage && !isPopping)
        {
            StartCoroutine(LevelUpPop());
        }
    }

    private void UpdateShake()
    {
        if (currentStage >= 3)
        {
            rectTransform.anchoredPosition = startPosition;
            return;
        }

        if (currentFill < shakeStartPercent || isPopping)
        {
            rectTransform.anchoredPosition = startPosition;

            return;
        }

        float shakeProgress = Mathf.InverseLerp(shakeStartPercent, 1f, currentFill);
        float strength = Mathf.Lerp(minShakeStrength, maxShakeStrength, shakeProgress);
        float speed = Mathf.Lerp(minShakeSpeed, maxShakeSpeed, shakeProgress);
        float x = Mathf.Sin(Time.time * speed) * strength;
        float y = Mathf.Cos(Time.time * speed * 1.3f) * strength;

        rectTransform.anchoredPosition = startPosition + new Vector2(x, y);
    }

    private IEnumerator LevelUpPop()
    {
        isPopping = true;
        rectTransform.anchoredPosition = startPosition;
        Vector3 targetScale = startScale * popScale;
        float timer = 0f;

        while (timer < popDuration)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / popDuration);
            float easedT = t * t * t;

            rectTransform.localScale = Vector3.Lerp(startScale, targetScale, easedT);
            
            yield return null;
        }

        timer = 0f;

        while (timer < popDuration)
        {
            timer += Time.deltaTime;

            float t = Mathf.Clamp01(timer / popDuration);
            float easedT = 1f - Mathf.Pow(1f - t, 3f);
            rectTransform.localScale = Vector3.Lerp(targetScale, startScale, easedT);

            yield return null;
        }

        rectTransform.localScale = startScale;

        isPopping = false;
    }

    private void SetSkeleton(int level)
    {
        dangerSkeletonLevel1.enabled = level == 1;
        dangerSkeletonLevel2.enabled = level == 2;
        dangerSkeletonLevel3.enabled = level == 3;
        dangerSkeletonLevel4.enabled = level == 4;
    }
}