using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
public class Lifeforce : MonoBehaviour
{
    [SerializeField] private List<string> beats = new List<string>();
    private List<string> notes = new List<string> { "note", "rest", "slide"};
    [SerializeField] private AudioSource audioSource;
    private float bpm = 120f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        CreateMeasure();
        StartCoroutine(PlayBeatSequence());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator PlayBeatSequence()
    {
        for (int i = 0; i < beats.Count; i++)
        {
            if (beats[i] == "note")
            {
                audioSource.Play();

            }
            else if (beats[i] == "slide")
            {
                audioSource.Play();
            }
 
            
            yield return new WaitForSeconds(60 / bpm);
            Debug.Log(beats[i]);
        

        }
        CreateMeasure();
        StartCoroutine(PlayBeatSequence());
    }

    void CreateMeasure()
    {
        int totalBeats = 16;
        beats = new List<string>();
        for (int i = 0; i < totalBeats; i++)
        {
            string randomNote = notes[Random.Range(0, notes.Count)];

            if (randomNote == "slide")
            {
                beats.Add(randomNote);
                int slideLength = Random.Range(1, totalBeats - i + 1);
                for (int j = 0; j < slideLength; j++)
                {
                    beats.Add("sliding");
                }
                i += slideLength;
            }
            else
            {
                beats.Add(randomNote);
                int sectionLength = Random.Range(1, totalBeats - i + 1);
                for (int j = 0; j < sectionLength; j++)
                {
                    beats.Add(randomNote);
                }
                i += sectionLength;
            }
            

        }
        
        

    }
}
