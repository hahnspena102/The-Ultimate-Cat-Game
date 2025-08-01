using UnityEngine;
using UnityEngine.EventSystems;

public class TreeZoom : MonoBehaviour, IScrollHandler
{
    public RectTransform content;
    public float zoomSpeed = 0.1f;
    public float minScale = 0.5f;
    public float maxScale = 2f;

    void Update()
    {
        // Pinch zoom (touch)
        if (Input.touchCount == 2)
        {
            Touch touch0 = Input.GetTouch(0);
            Touch touch1 = Input.GetTouch(1);
            
            Vector2 touch0PrevPos = touch0.position - touch0.deltaPosition;
            Vector2 touch1PrevPos = touch1.position - touch1.deltaPosition;
            
            float prevTouchDeltaMag = (touch0PrevPos - touch1PrevPos).magnitude;
            float touchDeltaMag = (touch0.position - touch1.position).magnitude;
            
            float deltaMagnitudeDiff = touchDeltaMag - prevTouchDeltaMag;
            Zoom(deltaMagnitudeDiff * 0.01f);
        }
    }

    public void OnScroll(PointerEventData eventData)
    {
        Zoom(eventData.scrollDelta.y * zoomSpeed);
    }

    void Zoom(float increment)
    {
        float currentScale = content.localScale.x;
        float targetScale = Mathf.Clamp(currentScale + increment, minScale, maxScale);
        content.localScale = new Vector3(targetScale, targetScale, 1);
    }
}
