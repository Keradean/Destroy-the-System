using System;
using UnityEngine;

namespace Project.Scripts.Dennis.VFX
{
    // Briefkasten für Effekte: Skripte rufen RaiseVFX auf, der VFXManager hört zu.
    // Basiert auf Chris' VFXEventChannel.
    [CreateAssetMenu(fileName = "VFXEventChannel", menuName = "Destroy the System/VFX/VFX Event Channel")]
    public class VFXEventChannel : ScriptableObject
    {
        public event Action<VFXData, Vector3, Quaternion> OnVFXRequested;

        public void RaiseVFX(VFXData data, Vector3 position, Quaternion rotation)
        {
            if (data == null) return;
            OnVFXRequested?.Invoke(data, position, rotation);
        }

        public void RaiseVFX(VFXData data, Vector3 position)
        {
            RaiseVFX(data, position, Quaternion.identity);
        }
    }
}
