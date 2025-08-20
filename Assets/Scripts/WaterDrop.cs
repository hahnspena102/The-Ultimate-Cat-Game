using UnityEngine;

public class WaterDrop : MonoBehaviour
{
    
    [SerializeField] private TMPro.TextMeshProUGUI textBox;
    [SerializeField] private int point;
    [SerializeField] private Sprite goldDropSprite;
    [SerializeField] private bool isGold;
    private Rigidbody2D rb;

    private bool isPoison = false;

    private SpriteRenderer sr;
    private float pointScale = 0.005f;


    public global::System.Int32 Point { get => point; set => point = value; }
    public global::System.Boolean IsGold { get => isGold; set => isGold = value; }

    //ABFF6B

    void Start()
    {
        if (point == 0) Destroy(gameObject);
        if (point < 0)
        {
            isPoison = true;
        }

        sr = GetComponent<SpriteRenderer>();

        if (sr != null && isPoison)
        {
            sr.color = new Color(0.67058824f, 1f, 0.41960784f, 1f);

            if (textBox != null) textBox.color = new Color(0.13725490f, 0.33725490f, 0.26274510f, 1f);
        }

        if (sr != null && isGold)
        {
            sr.color = new Color(1f, 1f, 1f);
            sr.sprite = goldDropSprite;

            if (textBox != null) textBox.color = new Color(0.639f, 0.318f, 0.145f, 1.000f);
        }

        if (textBox != null)
        {
            textBox.text = $"{point}";
        }

        transform.localScale = new Vector3(1 + Mathf.Abs(point) * pointScale, 1 + Mathf.Abs(point) * pointScale, 1);

        Destroy(gameObject, 3f);
    }   

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            Destroy(gameObject);
        }
    }
}
