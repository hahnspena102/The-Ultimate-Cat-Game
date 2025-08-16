using System.Collections;
using UnityEngine;
using TMPro;

public class Popup : MonoBehaviour
{
    [SerializeField]private TextMeshProUGUI textMesh;

    private float moveSpeed = 4f;
    private float fadeDuration = 1.5f;
    
    private Color originalColor;
    private int number;
    private Color outlineColor = Color.black;

    public global::System.Int32 Number { get => number; set => number = value; }
    public Color OutlineColor { get => outlineColor; set => outlineColor = value; }

    void Start()
    {
        originalColor = textMesh.color;
        StartCoroutine(FadeOutAndMoveUp());
        if (number > 0)
        {
            textMesh.text = $"+{number}";
            textMesh.fontMaterial.SetColor("_OutlineColor", outlineColor);
        }
        else if (number < 0)
        {
            textMesh.text = $"{number}";
            textMesh.fontMaterial.SetColor("_FaceColor", new Color(0.788f, 0.247f, 0.247f, 1f));
            textMesh.fontMaterial.SetColor("_OutlineColor", new Color(0.278f, 0.106f, 0.106f, 1.000f));
        }
        else
        {
            textMesh.text = $"{number}";
            textMesh.fontMaterial.SetColor("_OutlineColor", outlineColor);
        }
        

        
        //textMesh.fontMaterial.SetFloat("_OutlineWidth", 1f);
    }

    private IEnumerator FadeOutAndMoveUp()
    {
        float elapsedTime = 0f;
        Vector2 startPosition = transform.position;

        while (elapsedTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            textMesh.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            transform.position = startPosition + new Vector2(0, moveSpeed * (elapsedTime / fadeDuration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }
}