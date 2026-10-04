using System;
using System.Collections.Generic;
using UnityEngine;

public class FXBridge : MonoBehaviour
{
    public enum FXType
    {
        ParticleSystem,
        GameObjectToggle,
        AudioSource
    }

    public enum ParticleAction
    {
        Play,
        StopEmitting,
        StopAndClear
    }

    [Serializable]
    public struct FXBinding
    {
        public string eventName;
        public FXType type;
        
        [Header("Targets")]
        public ParticleSystem particle;
        public GameObject targetObject;
        public AudioSource audioSource;

        [Header("Settings")]
        public ParticleAction particleAction;
        public bool toggleState;
    }

    [Header("Event Bindings")]
    [SerializeField] private List<FXBinding> fxBindings = new List<FXBinding>();

    private readonly Dictionary<string, List<FXBinding>> bindingLookup = new Dictionary<string, List<FXBinding>>();

    private void Awake()
    {
        BuildLookupDictionary();
    }

    private void BuildLookupDictionary()
    {
        bindingLookup.Clear();

        for (int i = 0; i < fxBindings.Count; i++)
        {
            var binding = fxBindings[i];
            if (string.IsNullOrEmpty(binding.eventName)) continue;

            if (!bindingLookup.TryGetValue(binding.eventName, out var list))
            {
                list = new List<FXBinding>();
                bindingLookup.Add(binding.eventName, list);
            }
            list.Add(binding);
        }
    }

    public void OnAnimationEvent(string eventName)
    {
        if (!bindingLookup.TryGetValue(eventName, out var bindings)) return;

        for (int i = 0; i < bindings.Count; i++)
        {
            ExecuteBinding(bindings[i]);
        }
    }

    private void ExecuteBinding(FXBinding binding)
    {
        switch (binding.type)
        {
            case FXType.ParticleSystem:
                if (binding.particle == null) break;

                switch (binding.particleAction)
                {
                    case ParticleAction.Play:
                        if (!binding.particle.gameObject.activeSelf)
                            binding.particle.gameObject.SetActive(true);

                        binding.particle.Play(true);
                        break;

                    case ParticleAction.StopEmitting:
                        binding.particle.Stop(true, ParticleSystemStopBehavior.StopEmitting);
                        break;

                    case ParticleAction.StopAndClear:
                        binding.particle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                        break;
                }
                break;

            case FXType.GameObjectToggle:
                if (binding.targetObject != null)
                {
                    binding.targetObject.SetActive(binding.toggleState);
                }
                break;

            case FXType.AudioSource:
                if (binding.audioSource != null)
                {
                    binding.audioSource.Play();
                }
                break;
        }
    }
}