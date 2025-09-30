using Unity.VisualScripting;
using UnityEngine;

public class UsableLandScript : MonoBehaviour
{
    // when player approaches this object, they may plant something. 
    // this object needs to handle initialization process, and the growth is gonna be handled by plant

    public GameObject player;
    public PlayerScript playerScript;

    public GameObject carrot, wheat, corn;

    public GameObject currentPlant = null;
    public PlantScript currentPlantScript = null;

    // Units
    int kgSoilPerM2 = 260;
    int NitrogenMgPerM2 = 13000;

    int ppmOfNutrientsPerFert = 10;

    int KG_SOIL_PER_M2 = 260;
    int BLOCK_AREA = 72;
    int KG_SOIL_PER_BLOCK = 18720;
    // recommended values
    // 280800 min - 936000mg - max 3.7e6 nitrogen per block
    // 187200 min - 936000mg - max 5.6e6 phosphorus per block
    // 2.8e6 min - 5616000mg - 18.7e6 mg potassium per block

    // grams consumed per day on a 72 m² corn tile (average across season)
    // int needN_g_perDay = 11;
    // int needP_g_perDay = 2;
    // int needK_g_perDay = 8;

    // 33
    // 6
    // 24
    // for 3 days, total of 63g of nutrients is consumed

    // public int nitrogen_mg = 

    // float fertMg = 210;

    // Nutrition variables in mg per 1 kg of soil
    // begins with poor values
                                    // worst value - perfect value - max value
    public int ppm_nitrogen = 26; // 15 - 50 - 200
    public int ppm_phosphorus = 17; // 10 - 50 - 300 
    public int ppm_potassium = 210; // 150 - 300 - 1000

    // 100 is ideal. the more far, the more bad for plant
    // will decrease by time and by nutrientRetention
    float nutrientConsumptionPerDay = 1f;

    // out of 100, how great the soil retains the nutrients. 
    // 100 - doesnt lose a single bit even in runoff and doesnt even decrease, 
    // 50 - will lose half of what it has in runoff. can never decrease and always <100
    public float nutrientRetention = 0.97f; // 0.97 - after 10 days nutrient level goes from 100 to 73

    // 20 secs for plant to reach its max growth
    // 120 days

    public bool interactable = false;

    int fertPrize = 10;
    int coverCropPrice = 15;

    float _t = 0; // sec passed until last day
    int secPerDay = 5;

    void Awake()
    {
        player = GameObject.FindWithTag("Player");
        playerScript = player.GetComponent<PlayerScript>();
        Debug.Log(playerScript.money);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

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

        if (Input.GetKeyDown(KeyCode.P))
        {
            Plant(carrot);
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            HarvestAndSell();
        }

        if (playerScript.money >= fertPrize && (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))) // type 1 1 5
        {
            playerScript.money -= fertPrize;
            ppm_nitrogen += 43;
            ppm_phosphorus += 43;
            ppm_potassium += 214;
        }
        if (playerScript.money >= fertPrize && (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))) // type 1 3 6
        {
            playerScript.money -= fertPrize;
            ppm_nitrogen += 30;
            ppm_phosphorus += 90;
            ppm_potassium += 180;
        }
        if (playerScript.money >= fertPrize && (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))) // type 2 1 1
        {
            playerScript.money -= fertPrize;
            ppm_nitrogen += 150;
            ppm_phosphorus += 75;
            ppm_potassium += 75;
        }
        if (Input.GetKeyDown(KeyCode.Alpha9) || Input.GetKeyDown(KeyCode.Keypad9)) // buy soilRetention
        {
            if (playerScript.money >= coverCropPrice)
            {
                playerScript.money -= coverCropPrice;
                nutrientRetention += 0.001f;
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3)) // type 1 1 5
        {

        }
    }

    void SimOneDay()
    {
        currentPlantScript.SimOneDay();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "Player")
        {
            interactable = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.name == "Player")
        {
            interactable = false;
        }
    }

    // handles the planting process
    void Plant(GameObject plant)
    {
        if (!interactable)
        {
            return;
        }
        if (currentPlant != null)
        {
            Debug.Log("Land occupied.");
            return;
        }

        playerScript.money -= 75; // maybe different cost for each plant

        Debug.Log("Plant attempt");
        currentPlant = Instantiate(plant, transform);
        currentPlant.transform.SetParent(transform);
        currentPlantScript = currentPlant.GetComponent<PlantScript>();
        currentPlantScript.land = this;

        // currentPlantScript.nutrientRequirementsMet = 
        currentPlantScript.plantQuality = 100f * currentPlantScript.nutrientRequirementsMet;

        currentPlantScript.Init();
        Debug.Log("Planted!");
    }

    void HarvestAndSell()
    {
        if (!interactable)
        {
            return;
        }
        if (currentPlant == null)
        {
            Debug.Log("Nothing to harvest.");
            return;
        }

        float growthRatio = currentPlantScript.growthStage / 7f;
        int price = Mathf.RoundToInt(growthRatio * currentPlantScript.plantQuality);

        playerScript.money += price;
        Debug.Log("Sold for " + price);
        Destroy(currentPlant);
        currentPlant = null;
        currentPlantScript = null;
    }
}
