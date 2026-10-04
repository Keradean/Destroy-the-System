using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Project.Scripts.Philon.Levelup
{
    public class LevelUpManager : MonoBehaviour
    {
        [SerializeField] GameObject LevelUpScreen;
        [SerializeField] Image[] currentImages = new Image[3];

        [SerializeField] List<Sprite> options;

        private int[] currentOptions = new int[3];


        [ContextMenu("Level Up")]
        public void LevelUp()
        {
            //Pause Game
            GenerateOptions();
            LevelUpScreen.SetActive(true);
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
                    break;
                case 1:
                    // Upgrade 2 Collection Range
                    Debug.Log("Upgrade 2 Collection Range");
                    break;
                case 2:
                    // Upgrade 3 Damage
                    Debug.Log("Upgrade 3 Damage");
                    break;
                case 3:
                    // Upgrade 4 Projectile
                    Debug.Log("Upgrade 4 Projectile");
                    break;
                case 4:
                    // Upgrade 5 Projectile Bounce
                    Debug.Log("Upgrade 5 Projectile Bounce");
                    break;
                case 5:
                    // Upgrade 6 Speed
                    Debug.Log("Upgrade 6 Speed");
                    break;
                default:
                    Debug.LogError("Invalid upgrade option index: " + optionIndex);
                    break;
            }

            LevelUpScreen.SetActive(false);
        }
    }
}