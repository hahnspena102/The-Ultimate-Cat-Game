using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Energy : MonoBehaviour
{
    [SerializeField] private DataObject dataObject;
    [SerializeField] private List<GameObject> projectiles = new List<GameObject>();
    [SerializeField] private GameObject player;
    [SerializeField] private TMPro.TextMeshProUGUI respawnTimer;
    private float spawnWidth = 4.5f;
    private float timeMin = 0.1f;
    private float timeMax = 2f;
    private bool isAlive = true;
    private float timeElapsed;
    private int growthMax = 10;
    private float growthRate = 0.5f;

    void Start()
    {
        StartCoroutine(ProjGenerator());
    }

    // Update is called once per frame
    void Update()
    {
        isAlive = dataObject.PlayerData.GameData.EnergyRespawn.Value ==
            dataObject.PlayerData.GameData.EnergyRespawn.MaxValue;

        if (player) player.SetActive(isAlive);
        if (respawnTimer)
        {
            if (!isAlive)
            {
                respawnTimer.text = $"Your cat has awaken! \nDream starting in {15 - dataObject.PlayerData.GameData.EnergyRespawn.Value} seconds...";
            }
            respawnTimer.gameObject.SetActive(!isAlive);
        }

        if (!isAlive) timeElapsed = 0;
        timeElapsed += Time.deltaTime;

        float energyValue = timeElapsed * growthRate;
        dataObject.PlayerData.GameData.UpdateStat("Energy", (int)Mathf.Round(Mathf.Min(energyValue, growthMax)));


    }

    IEnumerator ProjGenerator()
    {
        while (!isAlive)
        {
            yield return null;
        }
        
        GenerateProj();
        yield return new WaitForSeconds(Random.Range(timeMin, timeMax));
        StartCoroutine(ProjGenerator());
    }

    void GenerateProj()
    {
        if (projectiles.Count == 0) return;
        GameObject newProj = Instantiate(projectiles[Random.Range(0,projectiles.Count)], transform.position, Quaternion.identity);
        newProj.transform.SetParent(transform);

        if (newProj == null) return;

        Vector2 randomScreenPoint = new Vector2(13,
           Random.Range(0 - spawnWidth, 0 + spawnWidth + 1)
       );

        newProj.transform.localPosition = randomScreenPoint;
    }
}
