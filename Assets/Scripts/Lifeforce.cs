using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
public class Lifeforce : MonoBehaviour
{
    [SerializeField] private DataObject dataObject;
    [SerializeField] private List<string> beats = new List<string>();
    [SerializeField] private List<GameObject> beatObjects;
    private List<string> notes = new List<string> { "note", "rest", "slide" };
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private GameObject slidePrefab;
    [SerializeField] private GameObject xPrefab;

    [SerializeField] private Slider barSlider;
    [SerializeField] private Transform handle;

    [SerializeField]private bool animationPlaying = false;
    private List<GameObject> slides = new List<GameObject>();
    private List<GameObject> effects = new List<GameObject>();

 
    private float bpm = 160f;

    private int slidesIndex = 0;
    private Coroutine sliderAnimation;
    private int currentBeat;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(LifeforceCoroutine());
        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(handle.position);

        if (slides.Count <= 0 || slidesIndex >= slides.Count) return;



        RectTransform rectTransform = slides[slidesIndex].GetComponent<RectTransform>();

        Vector3[] corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);

        //Debug.Log(corners[0]);
        if (Mathf.Abs(handle.position.x - corners[0].x) > 1f)
        {
            return;
        }

        if (animationPlaying) return;

        animationPlaying = true;

        Debug.Log("start!");

        int slideLength = 0;
        for (int j = currentBeat; j < beats.Count; j++)
        {
            if (beats[j] == "sliding")
            {
                slideLength += 1;
            }
            else if (beats[j] == "sliding_last")
            {
                slideLength += 1;
                break;
            }
        }
        Slider s = slides[slidesIndex].GetComponent<Slider>();
        if (s != null)
        {
            sliderAnimation = AnimateSlider(s, 60 * slideLength / bpm, true);
        }
                
    }

    IEnumerator LifeforceCoroutine()
    {
        yield return null;
        barSlider.value = 0;
        CreateMeasure();
        yield return new WaitForSeconds((60 / bpm) * 4);
        StartCoroutine(PlayBeatSequence());
    }

    IEnumerator PlayBeatSequence()
    {
        // ACTUAL
        AnimateSlider(barSlider, (60 / bpm) * beats.Count);
        List<bool> successList = new List<bool>();

        for (int i = 0; i < beats.Count; i++)
        {
            bool success = false;
            float beatDuration = 60f / bpm;
            float timer = 0f;
            bool isComplete = false;

            currentBeat = i;

            while (timer < beatDuration)
            {
                if (beats[i] == "note")
                {
                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        if (!isComplete)
                        {
                            dataObject.PlayerData.GameData.UpdateStat("Lifeforce", 30);
                            success = true;
                            Image image = beatObjects[i].GetComponent<Image>();
                            if (image)
                                image.color = new Color(1f, 1f, 1f, 1f);
                            isComplete = true;
                        }


                    }
                }
                else if (beats[i] == "slide")
                {

                    if (!isComplete)
                    {
                        if (Input.GetKeyDown(KeyCode.Space))
                        {
                            Slider s = slides[slidesIndex].GetComponent<Slider>();

                            ColorBlock cb = s.colors;
                            cb.disabledColor = new Color(1f, 1f, 1f, 1f);
                            s.colors = cb;

                            dataObject.PlayerData.GameData.UpdateStat("Lifeforce", 30);
                            success = true;
                            Image image = beatObjects[i].GetComponent<Image>();
                            if (image)
                                image.color = new Color(1f, 1f, 1f, 1f);
                            isComplete = true;
                        }

                    }
                }
                else if (beats[i] == "sliding")
                {
                    success = true;
                    if (!Input.GetKey(KeyCode.Space) || !successList[i - 1])
                    {
                        //dataObject.PlayerData.GameData.UpdateStat("Lifeforce", 30);
                        isComplete = true;
                        success = false;

                        if (sliderAnimation != null)
                        {
                            StopCoroutine(sliderAnimation);
                            animationPlaying = false;
                        }

                    }



                }
                else if (beats[i] == "sliding_last")
                {
                    success = false;
                    if (successList[i - 1])
                    {
                        //dataObject.PlayerData.GameData.UpdateStat("Lifeforce", 30);
                        isComplete = true;
                        success = true;

                        Image image = beatObjects[i].GetComponent<Image>();
                        if (image)
                            image.color = new Color(1f, 1f, 1f, 1f);
                    }



                }
                else
                {
                    success = true;
                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        if (!isComplete)
                        {
                            dataObject.PlayerData.GameData.UpdateStat("Lifeforce", -100);
                            success = false;
                            if (xPrefab)
                            {
                                GameObject newObj = Instantiate(xPrefab, beatObjects[i].transform.position, Quaternion.identity, transform);
                                effects.Add(newObj);
                            }
                            isComplete = true;
                        }



                    }
                }
                timer += Time.deltaTime;
                yield return null;
            }

            //Debug.Log($"Beat {i}: {success}");
            successList.Add(success);

            if (beats[i] == "sliding_last")
            {
                slidesIndex += 1;
            }

        }

        StartCoroutine(LifeforceCoroutine());
    }

    public Coroutine AnimateSlider(Slider slider, float duration, bool mark = false)
    {
        return StartCoroutine(AnimateSliderCoroutine(slider, duration, mark));
    }

    private IEnumerator AnimateSliderCoroutine(Slider slider, float duration, bool mark = false)
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

        if (mark) animationPlaying = false;
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
                        if (j == slideLength - 1)
                        {
                            beats.Add("sliding_last");
                        }
                        else
                        {
                            beats.Add("sliding");
                        }
                        
                        
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
        slidesIndex = 0;

        foreach (GameObject s in effects)
        {
            Destroy(s);
        }
        effects = new List<GameObject>();

        foreach (GameObject obj in beatObjects)
        {
            Image image = obj.GetComponent<Image>();
            if (image)
                image.color = new Color(0.60392157f, 0.51372549f, 0.47450980f, 1f);
        }

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
            else if (beats[i] == "sliding_last")
            {
                cg.alpha = 1f;

                Vector3 worldStart = previousSlide.transform.position;
                Vector3 worldEnd = beatObjects[i].transform.position;

                Vector3 localStart = transform.InverseTransformPoint(worldStart);
                Vector3 localEnd = transform.InverseTransformPoint(worldEnd);

                Vector3 localMid = (localStart + localEnd) / 2f;
                Vector3 direction = localEnd - localStart;
                float length = direction.magnitude;

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
