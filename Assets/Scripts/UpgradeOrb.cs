using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class UpgradeOrb : MonoBehaviour, ISelectHandler, IDeselectHandler
{

    [SerializeField] private Upgrade upgrade;
    [SerializeField] private bool isPurchased;
    [SerializeField] private DataObject dataObject;
    [SerializeField] private Outline outline;
    [SerializeField] private List<Transform> prerequisiteOrbs;

    [SerializeField] private GameObject linePrefab;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (isPurchased)
        {
            outline.effectColor = new Color(1f, 1f, 1f, 1f);
        }

        FindPrerequisiteOrbs();

        foreach (Transform prereq in prerequisiteOrbs)
        {
            DrawLine(prereq);
        }
    }

    public void FindPrerequisiteOrbs()
    {
        UpgradeOrb[] allOrbs = FindObjectsByType<UpgradeOrb>(FindObjectsSortMode.None);

        //prerequisiteOrbs.Clear();
        if (upgrade.Prerequisites.Count <= 0) return;
        
        foreach (Upgrade u in upgrade.Prerequisites)
        {
            if (u == null) continue;

            foreach (UpgradeOrb orb in allOrbs)
            {
                if (orb == null || orb == this || orb.upgrade == null) continue;

                if (orb.upgrade.Id == u.Id)
                {
                    prerequisiteOrbs.Add(orb.transform);
                    break;
                }
            }
        }
    }

    private void DrawLine(Transform prereqTransform)
    {
        Color lineColor = Color.white;

        Debug.Log("drawing line");

        GameObject newLine = Instantiate(linePrefab);
        newLine.transform.SetParent(prereqTransform.parent);
        newLine.transform.SetSiblingIndex(0);

        Image lineImage = newLine.GetComponent<Image>();
        lineImage.color = lineColor;

        RectTransform rt = newLine.GetComponent<RectTransform>();


        Vector3 start = transform.position;
        Vector3 end = prereqTransform.position;

        Vector3 direction = end - start;
        float distance = direction.magnitude;

        rt.sizeDelta = new Vector2(distance, 4f); 
        rt.anchorMin = rt.anchorMax = new Vector2(0, 0); 
        rt.pivot = new Vector2(0, 0.5f);

        rt.position = start;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        rt.rotation = Quaternion.Euler(0, 0, angle);
    }

    public void OnSelect(BaseEventData eventData)
    {
        Debug.Log("hey");
        dataObject.SelectedUpgrade = upgrade;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        Debug.Log("bye");
        dataObject.SelectedUpgrade = null;
    }
}
