using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class Love : MonoBehaviour
{
    [Header("Basics")]
    [SerializeField] private Animator animator;
    [SerializeField] private Canvas canvas;
    [SerializeField] private RectTransform canvasRectTransform;
    [SerializeField] private GameObject loveHeartPrefab;

    [Header("Values")]
    [SerializeField] private Values values;

    public Animator Animator { get => animator; set => animator = value; }
    public Canvas Canvas { get => canvas; set => canvas = value; }
    public RectTransform CanvasRectTransform { get => canvasRectTransform; set => canvasRectTransform = value; }

    void Start()
    {
        for (int i = 0; i < values.LoveHeartCount; i++)
        {
            Instantiate(loveHeartPrefab, canvas.transform.position, Quaternion.identity, canvas.transform); 
        }
    }
}