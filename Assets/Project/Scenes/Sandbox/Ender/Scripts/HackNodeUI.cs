using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HackNodeUI : MonoBehaviour
{
    [SerializeField] private Image hackProgress;
    [SerializeField] private GameObject hackedIcon;

    [Header("Complete Pop")]
    [SerializeField] private float popScale = 1.2f;
    [SerializeField] private float popDuration = 0.2f;
    [SerializeField] private float endScale = 1.08f;

    private float currentProgress = 0f;
    private bool isHacked = false;

    private Vector3 startScale;

    private void Awake()
    {
        startScale = transform.localScale;
    }

    private void Start()
    {
        hackProgress.fillAmount = 0f;
        hackedIcon.SetActive(false);
    }

    // Zeigt den aktuellen Hack-Fortschritt an.
    // Der Wert muss zwischen 0 und 1 liegen.
    // Beispiel: SetHackProgress(0.4f) = 40 %
    public void SetHackProgress(float progress)
    {
        if (isHacked)
            return;

        currentProgress = Mathf.Clamp01(progress);
        hackProgress.fillAmount = currentProgress;
    }

    // Schließt diesen UI-Slot ab.
    // Der Rahmen wird vollständig gefüllt und der Haken erscheint.
    public void CompleteHack()
    {
        if (isHacked)
            return;

        isHacked = true;
        currentProgress = 1f;
        hackProgress.fillAmount = 1f;
        hackedIcon.SetActive(true);

        StartCoroutine(CompletePop());
    }

    public bool IsHacked()
    {
        return isHacked;
    }

    private IEnumerator CompletePop()
    {
        Vector3 bigScale = startScale * popScale;
        Vector3 finalScale = startScale * endScale;
        float timer = 0f;

        while (timer < popDuration)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / popDuration);
            float easedT = t * t * t;
            transform.localScale = Vector3.Lerp(startScale, bigScale, easedT);

            yield return null;
        }

        timer = 0f;

        while (timer < popDuration)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / popDuration);
            float easedT = 1f - Mathf.Pow(1f - t, 3f);
            transform.localScale = Vector3.Lerp(bigScale, finalScale, easedT);

            yield return null;
        }

        transform.localScale = finalScale;
    }
}