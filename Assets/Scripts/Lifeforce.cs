using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
public class Lifeforce : MonoBehaviour
{
    [SerializeField] private List<string> beats = new List<string>();
    [SerializeField] private List<GameObject> beatObjects;
    private List<string> notes = new List<string> { "note", "rest", "slide" };
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private GameObject slidePrefab;

    [SerializeField] private Slider barSlider;
    private List<GameObject> slides = new List<GameObject>();

    
 
    private float bpm = 120f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(LifeforceCoroutine());
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator LifeforceCoroutine()
    {
        yield return null;
        CreateMeasure();
        StartCoroutine(PlayBeatSequence());
    }

    IEnumerator PlayBeatSequence()
    {
        AnimateSlider(barSlider, (60 / bpm) * beats.Count);
        yield return new WaitForSeconds(0.1f);

        for (int i = 0; i < beats.Count; i++)
        {
            if (beats[i] == "note")
            {
                audioSource.Play();
                Image image = beatObjects[i].GetComponent<Image>();
                if (image)
                {
                    image.color = new Color(1f, 1f, 1f, 1f);
                }


            }
            else if (beats[i] == "slide")
            {
                audioSource.Play();
            }


            yield return new WaitForSeconds(60 / bpm);

        }
        CreateMeasure();
        StartCoroutine(PlayBeatSequence());
    }

    public void AnimateSlider(Slider slider, float duration)
    {
        StartCoroutine(AnimateSliderCoroutine(slider, duration));
    }

    private IEnumerator AnimateSliderCoroutine(Slider slider, float duration)
    {
        float elapsed = 0f;
        slider.value = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            slider.value = Mathf.Clamp01(elapsed / duration);
            yield return null;
        }

        slider.value = 1f;
    }

    void CreateMeasure()
    {
        int totalBeats = 16;
        beats = new List<string>();
        int i = 0;

        while (i < totalBeats)
        {
            string randomNote = notes[Random.Range(0, notes.Count)];

            int remainingBeats = totalBeats - i;

            if (randomNote == "slide")
            {
                if (remainingBeats >= 2)
                {
                    int slideLength = Random.Range(2, remainingBeats + 1);
                    beats.Add("slide");
                    for (int j = 1; j < slideLength; j++)
                    {
                        beats.Add("sliding");
                    }
                    i += slideLength;
                }
                else
                {
                    beats.Add("note");
                    i++;
                }
            }
            else if (randomNote == "note")
            {
                beats.Add("note");
                i++;
            }
            else
            {
                int sectionLength = Random.Range(1, remainingBeats/2 + 1);
                for (int j = 0; j < sectionLength; j++)
                {
                    beats.Add(randomNote);
                }
                i += sectionLength;
            }
        }

        UpdatePanel();
    }


    void UpdatePanel()
    {
        foreach (GameObject s in slides)
        {
            Destroy(s);
        }
        slides = new List<GameObject>();

        GameObject previousSlide = null;
        
        for (int i = 0; i < beats.Count; i++)
        {
            CanvasGroup cg = beatObjects[i].GetComponent<CanvasGroup>();
            string next = "";
            if (i + 1 < beats.Count)
            {
                next = beats[i + 1];
            }

            if (cg == null) continue;

            if (beats[i] == "note")
            {
                cg.alpha = 1f;
            }
            else if (beats[i] == "slide")
            {
                cg.alpha = 1f;
                previousSlide = beatObjects[i];
            }
            else if (beats[i] == "sliding" && next != "sliding")
            {
                cg.alpha = 1f;

                Vector3 worldStart = previousSlide.transform.position;
                Vector3 worldEnd = beatObjects[i].transform.position;

                Vector3 localStart = transform.InverseTransformPoint(worldStart);
                Vector3 localEnd = transform.InverseTransformPoint(worldEnd);

                Vector3 localMid = (localStart + localEnd) / 2f;
                Vector3 direction = localEnd - localStart;
                float length = direction.magnitude;

                Debug.Log(length);

                GameObject newSlide = Instantiate(slidePrefab, transform);
                newSlide.transform.SetSiblingIndex(1);
                RectTransform slideRect = newSlide.GetComponent<RectTransform>();

                slideRect.anchoredPosition = localMid;

                slideRect.sizeDelta = new Vector2(length, slideRect.sizeDelta.y);

                slides.Add(newSlide);

            }
            else
            {
                cg.alpha = 0f;
            }
        }
    }
}
