using Project.Scenes.Sandbox.Ender.Scripts;
using Project.Scripts.Philon.Levelup;
using UnityEngine;

public class EXPHandler : MonoBehaviour
{
    [SerializeField] ExpBarUI expBarUI;
    [SerializeField] LevelUpManager levelUpManager;

    [SerializeField] private int currentLevel = 1;
    [SerializeField] private float currentExp = 0f;
    [SerializeField] private float expToNextLevel = 100f;

    [SerializeField] SphereCollider pickUpColl;

    public void AddExp(float amount)
    {
        currentExp += amount;

        while (currentExp >= expToNextLevel)
        {
            currentExp -= expToNextLevel;
            LevelUp();
        }
        if (expBarUI != null)
            expBarUI.SetExperience(currentLevel, currentExp, expToNextLevel);
    }

    private void LevelUp()
    {
        currentLevel++;
        expToNextLevel *= 1.1f; //Increase required EXP by 10%
        levelUpManager.LevelUp();
        Debug.Log($"Level Up! New Level: {currentLevel}");
    }

    public void SetPickupRange(float range)
    {
        pickUpColl.radius = range;
    }

}
