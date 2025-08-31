using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Soul : MonoBehaviour
{
    [SerializeField] private DataObject dataObject;
    [SerializeField] private GameObject ghostPrefab;
    [SerializeField] private List<GameObject> soulGhosts = new List<GameObject>();
    [SerializeField] private Canvas canvas;
    [SerializeField] private Camera targetCamera;
        [SerializeField] private Values values;
    private float ellipseWidth = 40f;
    private float ellipseHeight = 20f;
    private float timerMin = 3f;
    private float timerMax = 5f;

    public Canvas Canvas { get => canvas; set => canvas = value; }
    public Camera Camera { get => targetCamera; set => targetCamera = value; }

    void Start()
    {

        StartCoroutine(SpawnCoroutine());
        StartCoroutine(SoulCoroutine());

        foreach (SavedSoulGhost ssg in dataObject.PlayerData.GameData.SavedSoulGhosts)
        {
            GameObject newGhost = Instantiate(ghostPrefab, Vector3.zero, Quaternion.identity, transform);

            SoulGhost sg = newGhost.GetComponent<SoulGhost>();
            if (sg == null) return;

            sg.LoadSoulGhost(ssg);
            soulGhosts.Add(newGhost);
        }


    }

    IEnumerator SoulCoroutine()
    {
        dataObject.PlayerData.GameData.SoulBullets.Update(1);
        yield return new WaitForSeconds(values.ReloadRate);
        StartCoroutine(SoulCoroutine());
    }
        

    IEnumerator SpawnCoroutine()
    {
        SpawnGhost();
        yield return new WaitForSeconds(Random.Range(timerMin, timerMax));
        StartCoroutine(SpawnCoroutine());
    }

    void SpawnGhost()
    {
        float a = ellipseWidth / 2f;
        float b = ellipseHeight / 2f;

        float angle = Random.Range(0f, 2f * Mathf.PI);
        float x = a * Mathf.Cos(angle);
        float y = b * Mathf.Sin(angle);

        Vector3 spawnPos = transform.position + new Vector3(x, y, 0f);
        GameObject newGhost = Instantiate(ghostPrefab, spawnPos, Quaternion.identity, transform);
        newGhost.transform.SetParent(transform);

        SoulGhost sg = newGhost.GetComponent<SoulGhost>();
        sg.SetHealth(Random.Range(1, 6));


        soulGhosts.Add(newGhost);
    }

    void OnDestroy()
    {

        List<SavedSoulGhost> newList = new List<SavedSoulGhost>();
        foreach (GameObject go in soulGhosts)
        {
            if (go == null) continue;
            SoulGhost sg = go.GetComponent<SoulGhost>();
            if (sg == null) continue;
            newList.Add(sg.SaveSoulGhost());
        }
        dataObject.PlayerData.GameData.SavedSoulGhosts = newList;
    }

    public void RemoveGhost(GameObject ghost)
    {
        if (soulGhosts.Contains(ghost))
        {
            soulGhosts.Remove(ghost);
        }
    }
    
    public void ClearField()
    {
        foreach (GameObject go in soulGhosts)
        {
            SoulGhost sg = go.GetComponent<SoulGhost>();
            if (sg == null) return;
            sg.Health = 0;
            sg.Drops = false;
        }
    }
}
