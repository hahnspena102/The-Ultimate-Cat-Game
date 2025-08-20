using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class UpgradeOrb : MonoBehaviour, ISelectHandler
{

    [SerializeField] private Upgrade upgrade;
    [SerializeField] private bool isPurchased;
    [SerializeField] private DataObject dataObject;
    [SerializeField] private Outline outline;
    [SerializeField] private Image image;
    [SerializeField] private List<Transform> prerequisiteOrbs;

    [SerializeField] private GameObject linePrefab;
    [SerializeField] private Button button;
    private List<Image> prereqLines = new List<Image>();

    private bool hasPrerequisites = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        FindPrerequisiteOrbs();

        foreach (Transform prereq in prerequisiteOrbs)
        {
            DrawLine(prereq);
        }
    }

    void Update()
    {
        if (upgrade == null) return;

        CheckPrerequisites();

        isPurchased = dataObject.PlayerData.GameData.Upgrades[upgrade.Id];
        Color statColor = Color.white;
        if (UI.StatToColorMap != null)
        {
            statColor = UI.StatToColorMap[upgrade.Type];
        }



        button.interactable = !dataObject.PlayerData.GameData.GetIsLocked(upgrade.Type);



        if (dataObject.PlayerData.GameData.GetIsLocked(upgrade.Type))
        {
            image.color = new Color(1f, 1f, 1f, 0f);
            foreach (Image i in prereqLines)
            {
                i.color = new Color(1f, 1f, 1f, 0f);
            }
        }
        else if (!hasPrerequisites)
        {
            image.color = new Color(1f, 1f, 1f, 0.1f);
            foreach (Image i in prereqLines)
            {
                i.color = new Color(1f, 1f, 1f, 0f);
            }
        }
        else if (isPurchased)
        {
            outline.effectColor = statColor;
            image.color = Color.Lerp(Color.white, statColor, 0.7f);
            foreach (Image i in prereqLines)
            {
                i.color = statColor;
            }
        }
        else
        {
            outline.effectColor = new Color(1f, 1f, 1f, 0f);
            image.color = Color.white;
            foreach (Image i in prereqLines)
            {
                i.color = Color.white;
            }
        }

    
    }

    public void CheckPrerequisites()
    {
        hasPrerequisites = true;
        if (upgrade.Prerequisites.Count == 0)
        {
            hasPrerequisites = true;
            return;
        }
        foreach (Upgrade u in upgrade.Prerequisites)
        {
            if (!dataObject.PlayerData.GameData.Upgrades[u.Id])
            {
                hasPrerequisites = false;
                return;
            }
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

        //Debug.Log("drawing line");

        GameObject newLine = Instantiate(linePrefab);
        newLine.transform.SetParent(prereqTransform.parent);
        newLine.transform.SetSiblingIndex(0);

        Image lineImage = newLine.GetComponent<Image>();
        lineImage.color = lineColor;
        prereqLines.Add(lineImage);

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
        dataObject.SelectedUpgrade = upgrade;
    }

/*
            public void OnDeselect(BaseEventData eventData)
            {
                Debug.Log("bye");
                //dataObject.SelectedUpgrade = null;
            }
            */
}
