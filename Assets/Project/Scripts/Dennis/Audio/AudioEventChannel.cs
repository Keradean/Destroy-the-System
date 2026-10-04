using System;
using UnityEngine;

namespace Project.Scripts.Dennis.Audio
{
    // Briefkasten für Sounds: Skripte rufen RaiseSFX/RaiseMusic auf, der AudioManager hört zu.
    // So muss niemand den AudioManager selbst kennen. Basiert auf Chris' AudioEventChannel.
    [CreateAssetMenu(fileName = "AudioEventChannel", menuName = "Destroy the System/Audio/Audio Event Channel")]
    public class AudioEventChannel : ScriptableObject
    {
        public event Action<SoundData, Vector3> OnSFXRequested;
        public event Action<SoundData, float> OnMusicRequested;

        // Sound an einer Position in der Welt abspielen
        public void RaiseSFX(SoundData data, Vector3 position)
        {
            if (data == null) return;
            OnSFXRequested?.Invoke(data, position);
        }

        // Musik wechseln, fadeDuration in Sekunden
        public void RaiseMusic(SoundData data, float fadeDuration = 1f)
        {
            if (data == null) return;
            OnMusicRequested?.Invoke(data, fadeDuration);
        }
    }
}
