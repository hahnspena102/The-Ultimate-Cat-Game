using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ReceiptBlock : MonoBehaviour
{
    [SerializeField] private float delayBeforeFade = 3f;
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private List<AudioClip> audioClips = new List<AudioClip>();
    [SerializeField] private string type;

    private Renderer objRenderer;
    private CanvasGroup canvasGroup;
    private SpriteRenderer spriteRenderer;

    public global::System.String Type { get => type; set => type = value; }

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        if (audioClips != null && audioSource != null)
        {
            if (type == "Good")
            {
                audioSource.clip = audioClips[0];
            }
            else if (type == "Bad")
            {
                audioSource.pitch = Random.Range(1.00f - 0.10f, 1.00f + 0.10f);
                audioSource.clip = audioClips[1];
            }
            else if (type == "Miscellaneous")
            {
                audioSource.clip = audioClips[2];
            }
            audioSource.Play();
        }

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
