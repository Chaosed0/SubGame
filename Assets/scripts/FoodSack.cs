using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FoodSack : MonoBehaviour
{
    [SerializeField] private int maxCapacity = 3;
    [SerializeField] private bool cookingStatChangesCapacity = true;

    private int numOfFood = 0;
    private UnitStats unitStats;

    [System.Serializable]
    public class FoodCountChangedEvent : UnityEvent<int> { };
    public FoodCountChangedEvent onFoodCountChanged = new FoodCountChangedEvent();

    void Start()
    {
        unitStats = GetComponent<UnitStats>();
        SetMaxCapacity();
    }

    private void SetMaxCapacity()
    {
        if (cookingStatChangesCapacity)
        {
            maxCapacity = (int) (maxCapacity*unitStats.Cooking);
        }
    }

    public bool IsBagFull()
    {
        if (numOfFood >= maxCapacity)
            return true;
        return false;
    }

    public bool HasFood()
    {
        return numOfFood != 0;
    }

    public bool AddToSack()
    {
        if (numOfFood < maxCapacity)
        {
            numOfFood++;
            if (onFoodCountChanged != null) {
                onFoodCountChanged.Invoke(numOfFood);
            }
            return true;
        }
        return false;
    }

    public int RemoveFromSack()
    {
        int totalFood = numOfFood;
        numOfFood = 0;

        if (onFoodCountChanged != null) {
            onFoodCountChanged.Invoke(numOfFood);
        }

        return totalFood;
    }
}
