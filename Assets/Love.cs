using UnityEngine;

public class Love : MonoBehaviour
{
    [SerializeField] private DataObject dataObject;
    [SerializeField] private Transform heartTransform; 
    [SerializeField] private float moveDuration = 0.5f;
    [SerializeField] private RectTransform canvasRectTransform;

    private float canvasXPadding = 64f;
    private float canvasYPadding = 480f; 
    private bool isMoving = false;
    private Vector3 targetPosition;
    private float moveTimer = 0f;
    void Awake()
    {
        Vector2 randomScreenPoint = new Vector2(
           Random.Range(canvasXPadding, Screen.width - canvasXPadding),
           Random.Range(canvasYPadding, Screen.height - canvasYPadding)
       );

        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, randomScreenPoint, null, out localPoint);
        
        heartTransform.localPosition = new Vector3(localPoint.x, localPoint.y, 0f);
    }

    void Update()
    {
        if (isMoving)
        {
            moveTimer += Time.deltaTime;
            float t = moveTimer / moveDuration;
            heartTransform.localPosition = Vector3.Lerp(heartTransform.localPosition, targetPosition, t);

            if (t >= 1f)
            {
                isMoving = false;
            }
        }
    }


    public void HeartPress()
    {
        dataObject.PlayerData.GameData.UpdateStat("Love", 10);

        StartHeartMove();
    }

    private void StartHeartMove()
    {
        Vector2 randomScreenPoint = new Vector2(
            Random.Range(canvasXPadding, Screen.width - canvasXPadding),
            Random.Range(canvasYPadding, Screen.height - canvasYPadding)
        );

        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, randomScreenPoint, null, out localPoint);

        if (heartTransform != null)
        {
            targetPosition = new Vector3(localPoint.x, localPoint.y, 0f);
            moveTimer = 0f;
            isMoving = true;
        }
    }
}
