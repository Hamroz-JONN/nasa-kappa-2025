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

    // max value is 100 for these
    public int soilHealth = 67;
    public int nutritionLevel = 67;
    public int waterLevel = 67;

    public bool interactable = false;



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
        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
        {
            Plant(carrot);
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            HarvestAndSell();
        }
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
        currentPlantScript.Init();
        Debug.Log("Planted!");
    }

    void HarvestAndSell()
    {
        if (currentPlant == null)
        {
            Debug.Log("Nothing to harvest.");
            return;
        }
        if (!interactable)
        {
            return;
        }
        playerScript.money += (currentPlantScript.growthStage - 1) * 50;
        Debug.Log("Sold for " + (currentPlantScript.growthStage - 1) * 50 + " $");
        Destroy(currentPlant);
        currentPlant = null;
        currentPlantScript = null;
    }
}
