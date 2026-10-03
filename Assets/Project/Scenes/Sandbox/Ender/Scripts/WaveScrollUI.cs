using UnityEngine;
using UnityEngine.UI;

namespace Project.Scenes.Sandbox.Ender.Scripts
{
    [RequireComponent(typeof(RawImage))]
    public class WaveScrollUI : MonoBehaviour
    {
        [SerializeField] private float scrollSpeedX = 0.2f;
        [SerializeField] private float scrollSpeedY = 0f;

        private RawImage rawImage;
        private Rect uvRect;

        private void Awake()
        {
            rawImage = GetComponent<RawImage>();
            uvRect = rawImage.uvRect;
        }

        private void Update()
        {
            uvRect.x += scrollSpeedX * Time.deltaTime;
            uvRect.y += scrollSpeedY * Time.deltaTime;
            rawImage.uvRect = uvRect;
        }
    }
}