using UnityEngine;

namespace Project.Scripts.Dennis.Player
{
    // Sitzt am Objekt mit dem Animator und reicht die Animation Events an die Skripte am Player weiter.
    // Die Methodennamen müssen exakt zu Dominics Liste passen.
    public class PlayerAnimationEvents : MonoBehaviour
    {
        private PlayerAttack _attack;
        private PlayerHealthReaction _healthReaction;
        
        private void Awake()
        {
            _attack = GetComponentInParent<PlayerAttack>();
            _healthReaction = GetComponentInParent<PlayerHealthReaction>();
        }

        // Attack
        public void AE_OnChargeStartFrame() { }   // TODO: Lade-Effekt und Sound
        public void AE_OnShootFrame() { _attack.Shoot(); }
        public void AE_OnAttackEndFrame() { _attack.OnAttackFinished(); }

        // Damage
        public void AE_OnHitFlashFrame() { }      // TODO: Aufblitzen, Sound
        public void AE_OnDamageEndFrame() { }

        // Death
        public void AE_OnDeathStartFrame() { }    // TODO: Steuerung aus
        public void AE_OnDeathEndFrame() { _healthReaction.OnDeathAnimationFinished(); }      // TODO: Game Over

        // Spawn
        public void AE_OnSpawnStartFrame() { }
        public void AE_OnSpawnEndFrame() { }

        // Exit
        public void AE_OnExitStartFrame() { }     // TODO: Steuerung sperren
        public void AE_OnExitEndFrame() { }       // TODO: nächstes Level laden
    }
}