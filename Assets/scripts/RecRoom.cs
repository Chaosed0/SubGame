using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecRoom : MonoBehaviour {

    //Reference to the ship 
    public Ship ship;

    [SerializeField] private int maxFood = 6;
    private int currentFood = 1;

    [SerializeField] private float foodDestressAmount = 20;
    [SerializeField] private float foodEatTime = 4;
    private float foodEatCountdown;

    private Unit eatingUnit;


    void Start()
    {
        ship.onStationEntered.AddListener(OnEnterEffect);//Function with unit and tile type
        ship.onStationExited.AddListener(OnExitEffect);
        ResetFoodEatCountdown();
    }

    void Update()
    {
        if (eatingUnit != null && currentFood > 0)
        {
            foodEatCountdown -= Time.deltaTime;
            if (foodEatCountdown <= 0)
            {
                ResetFoodEatCountdown();
                TryEatFood();
            }
        }
	}

    private void ResetFoodEatCountdown()
    {
        foodEatCountdown = foodEatTime;
    }

    private void TryEatFood()
    {
        if (eatingUnit.EatFood(foodDestressAmount))
        {
            currentFood -= 1;
        }
    }

    void OnEnterEffect(Unit unit, TileType type)
    {
        FoodSack foodSack = unit.GetComponent<FoodSack>();
        if (foodSack.HasFood())
        {
            currentFood += foodSack.RemoveFromSack();
            currentFood = currentFood > maxFood ? maxFood : currentFood;
        }

        eatingUnit = unit;
        ResetFoodEatCountdown();
    }

    void OnExitEffect(Unit unit, TileType type)
    {

        eatingUnit = null;
        ResetFoodEatCountdown();
    }
}
