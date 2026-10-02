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
    [SerializeField] private float fullWidth = 408.6f;

    private void Start()
    {
        SetExperience(currentLevel, currentExp, expToNextLevel);
    }

    // Wird vom Gameplay-Code aufgerufen, wenn sich EXP ändert.
    // Beispiel: expBarUI.SetExperience(5, 35f, 100f);
    // level = aktuelles Spielerlevel
    // current = aktuelle EXP
    // required = benötigte EXP für das nächste Level
    
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
        expFill.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, fullWidth * expPercent);

        if (levelValue)
        {
            levelValue.text = currentLevel.ToString();
        }
    }
}