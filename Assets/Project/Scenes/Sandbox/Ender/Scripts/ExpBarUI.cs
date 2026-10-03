using UnityEngine;
using TMPro;

public class ExpBarUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private RectTransform expFill;
    [SerializeField] private TMP_Text levelValue;

    [Header("EXP")]
    [SerializeField] private int currentLevel = 1;
    [SerializeField] private float currentExp = 0f;
    [SerializeField] private float expToNextLevel = 100f;

    [Header("UI Settings")]
    [SerializeField] private float emptyPosX = -1715f;
    [SerializeField] private float fullPosX = -92f;

    private void Start()
    {
        SetExperience(currentLevel, currentExp, expToNextLevel);
    }

    // Beispiel: expBar.SetExperience(5, 35f, 100f);
    // level = aktuelles Spielerlevel - 5
    // current = aktuelle EXP - 35
    // required = benötigte EXP für das nächste Level - 100
    
    public void SetExperience(int level, float current, float required)
    {
        currentLevel = Mathf.Max(1, level);
        expToNextLevel = Mathf.Max(1f, required);
        currentExp = Mathf.Clamp(current, 0f, expToNextLevel);

        UpdateExpBar();
    }

    private void UpdateExpBar()
    {
        float expPercent = currentExp / expToNextLevel;
        float newPosX = Mathf.Lerp(emptyPosX, fullPosX, expPercent);
        Vector2 position = expFill.anchoredPosition;
        position.x = newPosX;
        expFill.anchoredPosition = position;

        if (levelValue)
        {
            levelValue.text = currentLevel.ToString();
        }
    }
}