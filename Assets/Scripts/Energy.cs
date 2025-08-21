using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Energy : MonoBehaviour
{
    [SerializeField] private DataObject dataObject;
    [SerializeField] private List<GameObject> projectiles = new List<GameObject>();
    [SerializeField] private GameObject particlePrefab;
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
    private int growthRate = 200;

    private UI ui;

    void Start()
    {
        GameObject go = GameObject.Find("UI");
        if (go) ui = go.GetComponent<UI>();

        StartCoroutine(ProjGenerator());
        StartCoroutine(ParticleGenerator());
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
                respawnTimer.text = $"Your cat has awaken! \nDream starting in {dataObject.PlayerData.GameData.EnergyRespawn.MaxValue - dataObject.PlayerData.GameData.EnergyRespawn.Value} seconds...";
            }
            respawnTimer.gameObject.SetActive(!isAlive);
        }

        if (!isAlive) timeElapsed = 0;
        timeElapsed += Time.deltaTime;

        if (timeElapsed > 3f)
        {
            CollectEnergy(growthRate, player.transform.position);
            timeElapsed = 0;
        }



    }

    IEnumerator ProjGenerator()
    {
        while (!isAlive)
        {
            yield return null;
        }

        GenerateEntity(projectiles[Random.Range(0, projectiles.Count)]);
        yield return new WaitForSeconds(Random.Range(timeMin, timeMax));
        StartCoroutine(ProjGenerator());
    }

    IEnumerator ParticleGenerator()
    {
        while (!isAlive || !dataObject.PlayerData.GameData.Upgrades[27])
        {
            yield return null;
        }


        GameObject particle = GenerateEntity(particlePrefab);
        if (particle)
        {
            EnergyParticle ep = particle.GetComponent<EnergyParticle>();
            if (dataObject.PlayerData.GameData.Upgrades[29])
            {
                particle.transform.localScale = new Vector3(0.3f, 0.3f, 1f);
                ep.EnergyValue = 500;
            }
            else if (dataObject.PlayerData.GameData.Upgrades[28])
            {
                particle.transform.localScale = new Vector3(0.2f, 0.2f, 1f);
                ep.EnergyValue = 250;
            }
            else
            {
                ep.EnergyValue = 100;
            }
            
        }
        yield return new WaitForSeconds(Random.Range(timeMin * 2f, timeMax * 2f));
        StartCoroutine(ParticleGenerator());
    }

    GameObject GenerateEntity(GameObject prefab)
    {
        GameObject newEntity = Instantiate(prefab, transform.position, Quaternion.identity);
        newEntity.transform.SetParent(transform);

        if (newEntity == null) return null;

        Vector2 randomScreenPoint = new Vector2(13,
           Random.Range(0 - spawnWidth, 0 + spawnWidth + 1)
       );

        newEntity.transform.localPosition = randomScreenPoint;

        return newEntity;
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

    public void CollectEnergy(int value, Vector3 pos)
    {
        dataObject.PlayerData.GameData.UpdateStat("Energy", value);
        dataObject.PlayerData.GameData.UpdatePoints(value);
        if (ui) ui.SpawnPopup(Camera.main.WorldToScreenPoint(pos), "Energy", value, canvas.transform);
        if (audioSource)
        {
            audioSource.clip = collectSFX;
            PlaySFX();
        }

    }

   
}
