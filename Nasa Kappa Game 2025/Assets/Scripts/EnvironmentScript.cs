using UnityEngine;
using System;

public class EnvironmentScript : MonoBehaviour
{
    float sustainability = 100f;
    public int emissionKg = 0;
    public float nutrientPpmRunoffed = 0;

    int fieldAmount = 10;

    float _tDrought = 0;
    int droughtDuration = 15;
    int droughtOccurency = 200;
    bool isDrought = false;

    int runoffOccurency = 120; // in seconds
    int rainDuration = 15; // in seconds
    float _tRain = 0; // time passed since last rain
    bool isRaining = false;

    GameObject[] grounds;
    GameObject cloud = null;

    public float lifetime = 0;

    public float runoffUntill = -1;
    public float droughtUntil = -1;

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
        _tRain += Time.deltaTime;
        _tDrought += Time.deltaTime;

        if (!isDrought && _tRain >= rainDuration && isRaining == true)
        {
            // StopRaining
            isRaining = false;
            cloud.SetActive(false);
        }
        if (!isDrought && _tRain >= runoffOccurency && isRaining == false)
        {
            _tRain = 0;

            cloud.SetActive(true);

            SimRain(); // not even considered
            SimRunoff();
            isRaining = true;
        }

        if (!isRaining && _tDrought > droughtDuration && isDrought == true)
        {
            _tDrought = 0;
            isDrought = false;
        }
        if (!isRaining && _tDrought > droughtOccurency && isDrought == false)
        {
            _tDrought = 0;
            isDrought = true;
        }

        // sustainability is always recalculated based on the values its given

        sustainability = 100f - Math.Min(0f, emissionKg - fieldAmount * 6) - nutrientPpmRunoffed;
        // nutrientPpmRunoffed;
        // emissionKg;
    }

    void SimRain()
    {
        // foreach (GameObject g in grounds)
        // {
        //     UsableLandScript script = g.GetComponent<UsableLandScript>();
        //     script.SimRain();
        // }
    }

    void SimRunoff()
    {
        runoffUntill = lifetime + rainDuration;
    }

    void SimDrought()
    {
        droughtUntil = lifetime + droughtDuration;
    }

    public static class ClimateClassifier
    {
        // Entry point
        public static string Classify(double lat, double lon)
        {
            // Normalize longitude to [-180, 180]
            lon = NormalizeLon(lon);

            // 1) Desert masks (override everything)
            if (IsDesert(lat, lon)) return "arid";

            // 2) Very cold high-latitude band (both hemispheres)
            if (Math.Abs(lat) >= 60.0) return "tundra";

            // 3) Known very-wet tropical regions (Amazon, Congo, Maritime SE Asia)
            if (IsRainyTropics(lat, lon)) return "tropical";

            // 4) Low-latitude default tropics (simple band)
            if (Math.Abs(lat) <= 15.0) return "tropical";

            // 5) Otherwise
            return "temperate";
        }

        // ---------- Helpers ----------

        private static double NormalizeLon(double lon)
        {
            // bring into [-180, 180]
            while (lon > 180) lon -= 360;
            while (lon < -180) lon += 360;
            return lon;
        }

        private static bool InBox(double lat, double lon, double latMin, double latMax, double lonMin, double lonMax)
        {
            return lat >= latMin && lat <= latMax && lon >= lonMin && lon <= lonMax;
        }

        private static bool IsDesert(double lat, double lon)
        {
            // --- Africa ---
            // Sahara (broad, includes Sahara core)
            if (InBox(lat, lon, 15, 32, -18, 35)) return true;
            // Arabian Peninsula (Rub' al Khali etc.)
            if (InBox(lat, lon, 12, 30, 34, 60)) return true;
            // Kalahari / Namib
            if (InBox(lat, lon, -30, -15, 10, 28)) return true;

            // --- Middle East / Central & South Asia ---
            // Thar Desert (India/Pakistan)
            if (InBox(lat, lon, 23, 32, 68, 78)) return true;
            // Karakum & Kyzylkum (Turkmenistan/Uzbekistan/Kazakhstan)
            if (InBox(lat, lon, 36, 46, 54, 70)) return true;
            // Iranian Plateau arid interior (broad brush)
            if (InBox(lat, lon, 25, 36, 45, 63)) return true;

            // --- East Asia ---
            // Gobi (cold desert, still "desert type")
            if (InBox(lat, lon, 37, 45, 90, 110)) return true;

            // --- Australia ---
            // Great Victoria / Great Sandy / Simpson (central arid)
            if (InBox(lat, lon, -30, -20, 120, 140)) return true;
            // Western Australian interior
            if (InBox(lat, lon, -30, -18, 114, 120)) return true;

            // --- North America ---
            // Mojave/Sonoran/Chihuahuan band (broad brush US SW + N Mexico)
            if (InBox(lat, lon, 24, 38, -117, -103)) return true;
            // Great Basin (dry interior)
            if (InBox(lat, lon, 36, 43, -120, -112)) return true;

            // --- South America ---
            // Atacama Desert (Chile)
            if (InBox(lat, lon, -30, -18, -75, -68)) return true;
            // Patagonian Steppe (dry; treat as desert type for gameplay)
            if (InBox(lat, lon, -50, -42, -72, -64)) return true;

            return false;
        }

        private static bool IsRainyTropics(double lat, double lon)
        {
            // Amazon Basin (very wet core)
            if (InBox(lat, lon, -15, 5, -75, -50)) return true;

            // Congo Basin (DRC region)
            if (InBox(lat, lon, -10, 5, 12, 31)) return true;

            // Maritime SE Asia (Indonesia/Malaysia/PNG tropics)
            if (InBox(lat, lon, -10, 10, 95, 150)) return true;

            // Western Africa Gulf of Guinea belt
            if (InBox(lat, lon, -5, 8, -10, 15)) return true;

            // Eastern India–Bangladesh–SE Asia monsoon core (lowland humid)
            if (InBox(lat, lon, 5, 23, 80, 105)) return true;

            // Central America tropical belt (Caribbean side)
            if (InBox(lat, lon, 7, 18, -92, -77)) return true;

            return false;
        }
    }

}

