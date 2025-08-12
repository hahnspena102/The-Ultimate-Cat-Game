using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
public class BindingsInfo : MonoBehaviour
{
    [SerializeField] private CanvasGroup bindingsGroup;
    private bool bindingsToggled = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            bindingsToggled = !bindingsToggled;
        }
        bindingsGroup.alpha = bindingsToggled ? 0.1f : 0f;
        
    }
}
