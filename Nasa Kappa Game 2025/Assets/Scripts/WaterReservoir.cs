using System;
using UnityEngine;

public class WaterReservoir : MonoBehaviour
{
    public int areaPlanted = 0;

    Transform waterLevel;

    public float currentWaterLevel = 0.6f;
    float previousWaterLevel = 0.6f;

    public float totalCapacity = 10000;

    float _t = 0; // sec passed until last day
    int secPerDay = 5; // should be same as UsableLandScript.secPerDay

    // every day 5L of water is used for 1 m^2
    int waterConsumedPerDayPerM2 = 5;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        waterLevel = transform.GetChild(0).transform;

        Vector3 newScale = waterLevel.localScale;
        newScale.y = currentWaterLevel;
        waterLevel.localScale = newScale;
    }

    // Update is called once per frame
    void Update()
    {
        _t += Time.deltaTime;
        if (_t > secPerDay)
        {
            _t = 0;
            SimOneDay();
        }

        currentWaterLevel = Math.Min(currentWaterLevel, 1f);
        if (previousWaterLevel != currentWaterLevel)
        {
            Vector3 newScale = waterLevel.localScale;
            newScale.y = currentWaterLevel;
            waterLevel.localScale = newScale;

            previousWaterLevel = currentWaterLevel;
        }
    }

    void SimOneDay()
    {
        currentWaterLevel = (totalCapacity * currentWaterLevel - areaPlanted * waterConsumedPerDayPerM2) / totalCapacity;
    }
}
