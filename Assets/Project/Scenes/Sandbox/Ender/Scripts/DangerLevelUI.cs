using UnityEngine;
using UnityEngine.UI;
using System.Collections;

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

    [Header("Danger Settings")]
    [SerializeField] private float maxDanger = 100f;
    [SerializeField] private float dangerPerSecond = 12.5f;
    [SerializeField] private float dangerPerNode = 30f;

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
    private float currentDanger = 0f;

    private RectTransform rectTransform;
    private Vector2 startPosition;
    private Vector3 startScale;

    private bool isPopping = false;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        startPosition = rectTransform.anchoredPosition;
        startScale = rectTransform.localScale;
    }

    private void Start()
    {
        dangerLevelOrange.fillAmount = 0f;
        dangerLevelRed.fillAmount = 0f;
        dangerLevelPurple.fillAmount = 0f;
        SetSkeleton(1);
    }

    private void Update()
    {
        if (currentStage >= 3)
            return;

        AddDanger(dangerPerSecond * Time.deltaTime);

        UpdateCurrentStage();
        UpdateShake();
    }

    public void AddDanger(float amount)
    {
        if (currentStage >= 3)
            return;

        currentDanger += amount;

        if (currentDanger >= maxDanger)
        {
            currentDanger = maxDanger;
            UpdateCurrentStage();
            LevelUpDanger();
        }
    }

    public void NodeHacked()
    {
        AddDanger(dangerPerNode);
    }

    private void LevelUpDanger()
    {
        currentStage++;
        currentDanger = 0f;

        rectTransform.anchoredPosition = startPosition;
        SetSkeleton(currentStage + 1);

        if (!isPopping)
        {
            StartCoroutine(LevelUpPop());
        }
    }

    private void UpdateCurrentStage()
    {
        float dangerPercent = Mathf.Clamp01(currentDanger / maxDanger);

        switch (currentStage)
        {
            case 0: dangerLevelOrange.fillAmount = dangerPercent;
                break;

            case 1: dangerLevelRed.fillAmount = dangerPercent;
                break;

            case 2: dangerLevelPurple.fillAmount = dangerPercent;
                break;
        }
    }

    private void UpdateShake()
    {
        float dangerPercent = Mathf.Clamp01(currentDanger / maxDanger);

        if (dangerPercent < shakeStartPercent || isPopping)
        {
            rectTransform.anchoredPosition = startPosition;
            return;
        }

        float shakeProgress = Mathf.InverseLerp(shakeStartPercent, 1f, dangerPercent);
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

    public int GetDangerLevel()
    {
        return currentStage + 1;
    }

    public float GetCurrentDanger()
    {
        return currentDanger;
    }
}