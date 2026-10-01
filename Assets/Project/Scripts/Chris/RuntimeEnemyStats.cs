using UnityEngine;
using System.Collections.Generic;
using System;

public class RuntimeEnemyStats : MonoBehaviour
{
    private EnemyDataSO baseData;
    private int currentStage;

    private float defenseMultiplier = 1.0f;
    private float damageMultiplier = 1.0f;

    public void Initialize(EnemyDataSO data, int stage)
    {
        baseData = data;
        currentStage = stage;
    }

    public float GetMaxHealth()
    {
        float stageScale = (float)(1f + (currentStage - 1) * 0.15);
        return baseData.baseHaealth * stageScale;
    }
    public float GetDamageOutput()
    {
        float stageScale = (float)(1f + (currentStage - 1) * 0.15);
        return baseData.baseDamage * stageScale;
    }
    public float GetIncomingDamageMultiplier()
    {
        return defenseMultiplier;
    }

}
