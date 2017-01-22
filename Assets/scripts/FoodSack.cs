using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSack : MonoBehaviour
{
    [SerializeField] private int maxCapacity = 3;
    [SerializeField] private bool cookingStatChangesCapacity = true;

    private int numOfFood = 0;
    private UnitStats unitStats;

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
            return true;
        }
        return false;
    }

    public int RemoveFromSack()
    {
        int totalFood = numOfFood;
        numOfFood = 0;
        return totalFood;
    }
}
