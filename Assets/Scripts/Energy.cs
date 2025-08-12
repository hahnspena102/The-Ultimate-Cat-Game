using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Energy : MonoBehaviour
{
    [SerializeField] private DataObject dataObject;
    [SerializeField] private List<GameObject> projectiles = new List<GameObject>();
    [SerializeField] private GameObject player;
    [SerializeField] private TMPro.TextMeshProUGUI respawnTimer;
    [SerializeField] private Canvas canvas;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip sleepSFX, awakenSFX, collectSFX;
    private float spawnWidth = 4.5f;
    private float timeMin = 0.1f;
    private float timeMax = 2f;
    private bool isAlive = true;
    private float timeElapsed;
    private int growthRate = 100;

    private UI ui;

    void Start()
    {
        GameObject go = GameObject.Find("UI");
        if (go) ui = go.GetComponent<UI>();

        StartCoroutine(ProjGenerator());
    }

    // Update is called once per frame
    void Update()
    {
        isAlive = dataObject.PlayerData.GameData.EnergyRespawn.Value ==
            dataObject.PlayerData.GameData.EnergyRespawn.MaxValue;

        if (player) player.SetActive(isAlive);

        if (respawnTimer.gameObject.activeSelf && dataObject.PlayerData.GameData.EnergyRespawn.Value == dataObject.PlayerData.GameData.EnergyRespawn.MaxValue)
        {
            audioSource.clip = sleepSFX;
            PlaySFX();
        }

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

        if (timeElapsed > 2f)
        {
            dataObject.PlayerData.GameData.UpdateStat("Energy", growthRate);
            dataObject.PlayerData.GameData.Points += Mathf.Max(growthRate, 0);
            if (ui) ui.SpawnPopup(Camera.main.WorldToScreenPoint(player.transform.position), "Energy", growthRate, canvas.transform);
            if (audioSource)
            {
                audioSource.clip = collectSFX;
                PlaySFX();
            }
            timeElapsed = 0;
        }



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
        GameObject newProj = Instantiate(projectiles[Random.Range(0, projectiles.Count)], transform.position, Quaternion.identity);
        newProj.transform.SetParent(transform);

        if (newProj == null) return;

        Vector2 randomScreenPoint = new Vector2(13,
           Random.Range(0 - spawnWidth, 0 + spawnWidth + 1)
       );

        newProj.transform.localPosition = randomScreenPoint;
    }

    public void PlayAwakenSFX()
    {
        audioSource.clip = awakenSFX;
        PlaySFX();
    }

    public void PlaySFX()
    {
        audioSource.pitch = Random.Range(1.00f - 0.10f, 1.00f + 0.10f);
        audioSource.Play();
    }
}
