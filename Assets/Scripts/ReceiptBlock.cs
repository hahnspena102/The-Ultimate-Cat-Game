using System.Collections;
using UnityEngine;

public class ReceiptBlock : MonoBehaviour
{
    [SerializeField] private float delayBeforeFade = 3f;
    [SerializeField] private float fadeDuration = 1f;

    private Renderer objRenderer;
    private CanvasGroup canvasGroup;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        StartCoroutine(FadeAndDestroy());
    }

    IEnumerator FadeAndDestroy()
    {
        yield return new WaitForSeconds(delayBeforeFade);

        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            float alpha = 1f - (elapsed / fadeDuration);

            if (canvasGroup != null)
            {
                canvasGroup.alpha = alpha;
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        if (canvasGroup != null) canvasGroup.alpha = 0f;

        Destroy(gameObject);
    }
}
