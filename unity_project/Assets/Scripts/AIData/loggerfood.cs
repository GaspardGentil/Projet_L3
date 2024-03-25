using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class FoodLogger : MonoBehaviour
{
    [Serializable]
    public class FoodData
    {
        public int id;
        public Vector3 position;
        public bool isSpawned;
    }

    // Table to store food data
    public List<FoodData> foodTable = new List<FoodData>();

    // Function to add food data to the table
    public void AddFoodData(int id, Vector3 position)
    {
        FoodData newFood = new FoodData();
        newFood.id = id;
        newFood.position = position;
        newFood.isSpawned = false; // By default, set spawned state to false
        foodTable.Add(newFood);
    }

    // Function to set the spawned state of a certain food ID
    public void SetSpawnedState(int id, bool isSpawned)
    {
        FoodData food = foodTable.Find(item => item.id == id);
        if (food != null)
        {
            food.isSpawned = isSpawned;
            if (!isSpawned)
            {
                StartCoroutine(ResetSpawnedStateAfterDelay(id));
            }
        }
        else
        {
            Debug.LogWarning("Food with ID " + id + " not found.");
        }
    }

    // Coroutine to reset spawned state after a delay
    private IEnumerator ResetSpawnedStateAfterDelay(int id)
    {
        yield return new WaitForSeconds(11f);
        SetSpawnedState(id, true);
    }

    // Function to get the spawned state of a certain food ID
    public bool GetSpawnedState(int id)
    {
        FoodData food = foodTable.Find(item => item.id == id);
        if (food != null)
        {
            return food.isSpawned;
        }
        else
        {
            Debug.LogWarning("Food with ID " + id + " not found.");
            return false;
        }
    }

    // Function to check if a position is already in the table
    public bool IsPositionInTable(Vector3 position)
    {
        foreach (FoodData food in foodTable)
        {
            if (food.position == position)
            {
                return true;
            }
        }
        return false;
    }

}
