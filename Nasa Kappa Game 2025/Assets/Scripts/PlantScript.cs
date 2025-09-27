using System.Collections.Generic;
using UnityEngine;

public class PlantScript : MonoBehaviour
{
    public int growthStage = 1; // 0 - nothing; 1 - unripe; 2 - almost done; 3 - ripe
    private float lifetime = 0;

    private SpriteRenderer SR;

    public float totalHealth;
    

    // [SerializeField] private List<Sprite> growthStageSprites;
    [SerializeField] private Sprite[] growthStageSprites = new Sprite[5];

    void Awake()
    {
        growthStage = 1;
        lifetime = 0;
        totalHealth = 80;
        SR = GetComponent<SpriteRenderer>();
    }

    public void Init()
    {
        SR.sprite = growthStageSprites[growthStage];
    }

    void Update()
    {
        // transform.localScale()

        lifetime += Time.deltaTime;

        if (growthStage < 3 && lifetime >= growthStage*5) {
            Debug.Log("Growth stage is now:" + growthStage);
            SR.sprite = growthStageSprites[++growthStage];
        }
    }
}
