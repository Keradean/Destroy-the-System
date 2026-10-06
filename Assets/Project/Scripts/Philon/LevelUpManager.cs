using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using Project.Scripts.Dennis.Player;

namespace Project.Scripts.Philon.Levelup
{
    public class LevelUpManager : MonoBehaviour
    {
        [SerializeField] GameObject LevelUpScreen;
        [SerializeField] Image[] currentImages = new Image[3];

        [SerializeField] List<Sprite> options;

        [SerializeField] PlayerSetup stats;
        [SerializeField] EXPHandler expHandler;
        [SerializeField] PlayerMoveComponent movement;
        [SerializeField] PlayerAttack attack;
        [SerializeField] PauseManager pauseManager;

        private int[] currentOptions = new int[3];

        private void Start()
        {
            if(attack == null) attack = movement.GetComponent<PlayerAttack>();
        }

        [ContextMenu("Level Up")]
        public void LevelUp()
        {
            //Pause Game
            GenerateOptions();
            LevelUpScreen.SetActive(true);
            pauseManager.SetPause();
        }

        private void GenerateOptions()
        {
            for (int i = 0; i < currentOptions.Length; i++)
            {
                currentOptions[i] = Random.Range(0, options.Count);
                // Check for duplicates and regenerate if necessary
                if (i != 0)
                {
                    while (currentOptions[0] == currentOptions[i])
                    {
                        currentOptions[i] = Random.Range(0, options.Count);
                    }
                    if (i == 2)
                    {
                        while (currentOptions[1] == currentOptions[i])
                        {
                            currentOptions[i] = Random.Range(0, options.Count);
                        }
                    }
                }

                currentImages[i].sprite = options[currentOptions[i]];
            }
        }

        public void SelectUpgrade(int index)
        {
            UpgradePlayer(currentOptions[index]);
        }

        private void UpgradePlayer(int optionIndex)
        {
            switch (optionIndex)
            {
                case 0:
                    // Upgrade 1 Attack Speed
                    Debug.Log("Upgrade 1 Attack Speed");
                    stats.Data.attackSpeed *= 1.1f; // Example: Increase attack speed by 10%
                    attack.UpdateAttackSpeed(); // Update the attack speed in PlayerAttack
                    break;
                case 1:
                    // Upgrade 2 Collection Range
                    Debug.Log("Upgrade 2 Collection Range");
                    stats.Data.pickupRange += 1f; // Example: Increase pickup range by 1 unit
                    expHandler.SetPickupRange(stats.Data.pickupRange); // Update the pickup range in EXPHandler
                    break;
                case 2:
                    // Upgrade 3 Damage
                    Debug.Log("Upgrade 3 Damage");
                    stats.Data.projectileDamage += 5f; // Example: Increase projectile damage by 5
                    break;
                case 3:
                    // Upgrade 4 Projectile
                    Debug.Log("Upgrade 4 Projectile");
                    stats.Data.projectileCount += 1; // Example: Increase projectile count by 1
                    break;
                case 4:
                    // Upgrade 5 Projectile Bounce
                    Debug.Log("Upgrade 5 Projectile Bounce");
                    stats.Data.projectileBounces += 1; // Example: Increase projectile bounces by 1
                    break;
                case 5:
                    // Upgrade 6 Speed
                    Debug.Log("Upgrade 6 Speed");
                    stats.Data.moveSpeed += 0.5f; // Example: Increase move speed by 1 unit
                    movement.SetMovespeed(stats.Data.moveSpeed); // Update the move speed in PlayerMoveComponent
                    break;
                default:
                    Debug.LogError("Invalid upgrade option index: " + optionIndex);
                    break;
            }

            LevelUpScreen.SetActive(false);
            pauseManager.SetPause();
        }
    }
}