using TMPro;
using UnityEngine;

public class RainScript : MonoBehaviour
{
    int runoffOccurency = 120; // in seconds
    int rainDuration = 15; // in seconds
    float _t = 0; // time passed since ...

    bool isRaining = false;

    GameObject[] grounds;
    GameObject cloud = null;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        grounds = GameObject.FindGameObjectsWithTag("Ground");

        cloud = transform.GetChild(0).gameObject;
        cloud.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        _t += Time.deltaTime;
        if (_t >= rainDuration && isRaining == true)
        {
            // StopRaining
            isRaining = false;
            cloud.SetActive(false);
        }
        if (_t >= runoffOccurency && isRaining == false)
        {
            _t = 0;

            cloud.SetActive(true);

            SimRain();
            SimRunoff();
            isRaining = true;
        }
    }

    void SimRain()
    {
        foreach (GameObject g in grounds)
        {
            UsableLandScript script = g.GetComponent<UsableLandScript>();
            script.SimRain();
        }
    }

    void SimRunoff()
    {
        foreach (GameObject g in grounds)
        {
            UsableLandScript script = g.GetComponent<UsableLandScript>();
            script.SimRunoff();
        }

    }
}
