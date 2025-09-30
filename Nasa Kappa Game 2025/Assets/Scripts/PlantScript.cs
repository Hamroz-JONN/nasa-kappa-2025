using System;
using System.Collections.Generic;
using UnityEngine;

public class PlantScript : MonoBehaviour
{
    private SpriteRenderer SR;
    public UsableLandScript land;

    // [SerializeField] private List<Sprite> growthStageSprites;
    [SerializeField] private Sprite[] growthStageSprites = new Sprite[5];

    public int growthStage; // 0 - nothing; 1 - unripe; 2 - almost done; 3 - ripe
    private float lifetime;

    public float nutrientRequirementsMet = 1; // the more away from 1, the more imperfect
    private bool hasWater;

    public float plantQuality;

    void Awake()
    {
        growthStage = 1;
        lifetime = 0;
        SR = GetComponent<SpriteRenderer>();

        hasWater = true;
    }

    public void Init()
    {
        SR.sprite = growthStageSprites[growthStage];
    }

    void Update()
    {
        lifetime += Time.deltaTime;

        if (growthStage < 3 && lifetime >= growthStage * 5)
        {
            Debug.Log("Growth stage is now:" + growthStage);
            SR.sprite = growthStageSprites[++growthStage];
        }
    }

    public void SimOneDay()
    {
        if (hasWater == false)
        {
            plantQuality -= 5;
        }
    }
}
