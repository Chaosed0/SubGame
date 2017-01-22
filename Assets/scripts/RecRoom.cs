using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RecRoom : MonoBehaviour {

    //Reference to the ship 
    public Ship ship;

    [SerializeField] private int maxFood = 6;
    private int currentFood = 1;

    [SerializeField] private float foodDestressAmount = 20;
    [SerializeField] private float foodEatTime = 4;
    private float foodEatCountdown;

    private Unit eatingUnit;

    public class OnFoodCountChanged : UnityEvent<int> { };
    public OnFoodCountChanged onFoodCountChanged = new OnFoodCountChanged();

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
            onFoodCountChanged.Invoke(currentFood);
        }
    }

    void OnEnterEffect(Unit unit, TileType type)
    {
        if (type == TileType.Rec) {
            FoodSack foodSack = unit.GetComponent<FoodSack>();
            if (foodSack.HasFood())
            {
                currentFood += foodSack.RemoveFromSack();
                currentFood = currentFood > maxFood ? maxFood : currentFood;
                onFoodCountChanged.Invoke(currentFood);
            }

            eatingUnit = unit;
            ResetFoodEatCountdown();
        }
    }

    void OnExitEffect(Unit unit, TileType type)
    {
        eatingUnit = null;
        ResetFoodEatCountdown();
    }
}
