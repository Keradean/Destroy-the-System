using UnityEngine;

namespace Project.Scripts.Dennis.Player
{
    // Liest beim Start die Werte aus dem PlayerDataSO und verteilt sie
    [RequireComponent(typeof(HealthComponent))]
    public class PlayerSetup : MonoBehaviour
    {
        [SerializeField] private PlayerDataSO _data;

        public PlayerDataSO Data => _data;

        private void Awake()
        {
            GetComponent<HealthComponent>().Initialize(_data.maxHealth);
        }
    }
}