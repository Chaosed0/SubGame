using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kitchen : MonoBehaviour {

    //Reference to the ship
    [SerializeField] private Ship ship;
    [SerializeField] private float foodRegenTime = 10.0f;
    [SerializeField] private float currentFoodRegenSpeed;
    [SerializeField] private bool cookingStatChangesFoodRegenRate = true;

    private float foodRegenCountdown;
    private FoodSack unitsFoodSack;

    public bool lostPower;

    void Start()
    {
        ship.onStationEntered.AddListener(OnEnterEffect);//Function with unit and tile type
        ship.onStationExited.AddListener(OnExitEffect);
        ResetCookingSpeed();
        ResetFoodRegenCountdown();
    }

    void Update()
    {
        if (unitsFoodSack != null && !unitsFoodSack.IsBagFull())
        {
            foodRegenCountdown -= Time.deltaTime * currentFoodRegenSpeed;
            if (foodRegenCountdown <= 0)
            {
                unitsFoodSack.AddToSack();
                ResetFoodRegenCountdown();
            }
        }
	}

    private void ResetFoodRegenCountdown()
    {
        foodRegenCountdown = foodRegenTime;
    }

    private void ResetCookingSpeed()
    {
        currentFoodRegenSpeed = 0;
    }

    void OnEnterEffect(Unit unit, TileType type)
    {
        if (lostPower == false)
        {
            unitsFoodSack = unit.GetComponent<FoodSack>();
            ResetFoodRegenCountdown();
            if (cookingStatChangesFoodRegenRate)
            {
                currentFoodRegenSpeed = unit.unitStats.Cooking;
            }
            else
            {
                currentFoodRegenSpeed = 1;
            }
        }
    }

    void OnExitEffect(Unit unit, TileType type)
    {
        if (lostPower == false)
        {
            unitsFoodSack = null;
            ResetCookingSpeed();
            ResetFoodRegenCountdown();
        }
    }
}
