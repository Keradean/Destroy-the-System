using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
public class WaveScrollUI : MonoBehaviour
{
    [SerializeField] private float scrollSpeedX = -0.2f;

    [Header("Vertical Pulse")]
    [SerializeField] private float pulseStrength = 0.2f;
    [SerializeField] private float pulseSpeed = 1.5f;

    private RawImage rawImage;
    private Rect uvRect;
    private Vector3 startScale;
    private float noiseOffset;

    private void Awake()
    {
        rawImage = GetComponent<RawImage>();
        uvRect = rawImage.uvRect;
        startScale = transform.localScale;
        noiseOffset = Random.Range(0f, 1000f);
    }

    private void Update()
    {
        uvRect.x += scrollSpeedX * Time.deltaTime;
        rawImage.uvRect = uvRect;
        float noise = Mathf.PerlinNoise(noiseOffset, Time.time * pulseSpeed);
        float scaleY = 1f + ((noise - 0.5f) * 2f * pulseStrength);
        transform.localScale = new Vector3(startScale.x, startScale.y * scaleY, startScale.z);
    }
}