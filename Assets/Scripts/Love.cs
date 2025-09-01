using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class Love : MonoBehaviour
{
    [Header("Basics")]
    [SerializeField] private Animator animator;
    [SerializeField] private Canvas canvas;
    [SerializeField] private GameObject loveHeartPrefab;
    [SerializeField] private RectTransform spawningRect;
    [SerializeField] private RectTransform canvasRect;

    [Header("Values")]
    [SerializeField] private Values values;
    private float ogWidth, ogHeight;

    public Animator Animator { get => animator; set => animator = value; }
    public Canvas Canvas { get => canvas; set => canvas = value; }
    public RectTransform SpawningRect { get => spawningRect; set => spawningRect = value; }
    public RectTransform CanvasRect { get => canvasRect; set => canvasRect = value; }

    void Awake()
    {
        ogWidth = spawningRect.sizeDelta.x;
        ogHeight = spawningRect.sizeDelta.y;
        if (values != null)spawningRect.sizeDelta = new Vector2(ogWidth * values.LoveRectXScale, ogHeight * values.LoveRectYScale);
        for (int i = 0; i < values.LoveHeartCount; i++)
        {
            Instantiate(loveHeartPrefab, canvas.transform.position, Quaternion.identity, canvas.transform);
        }

        
    }

}