using UnityEngine;

public class Love : MonoBehaviour
{
    [SerializeField] private DataObject dataObject;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void HeartPress()
    {
        dataObject.PlayerData.GameData.UpdateStat("Love", 10);
    }
}
