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
    public int AddFoodData(Vector3 position)
    {
        Debug.Log("Added new food at position: " + position);
        int newId = GenerateUniqueID();
        FoodData newFood = new FoodData();
        newFood.id = newId;
        newFood.position = position;
        newFood.isSpawned = false; // By default, set spawned state to false
        foodTable.Add(newFood);
        return newId;
    }

    // Function to generate a unique ID for the food
    private int GenerateUniqueID()
    {
        int newId;
        do
        {
            newId = UnityEngine.Random.Range(1, int.MaxValue); // Generate a random ID
        } while (foodTable.Exists(food => food.id == newId)); // Check if the ID already exists
        return newId;
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
        yield return new WaitForSeconds(5f);
        SetSpawnedState(id, true);
        Debug.Log("we think the food with ID: " + id + " has respawned");
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

    // Function to get the positions of currently spawned foods
    public List<Vector3> GetSpawnedFoodPositions()
    {
        List<Vector3> spawnedPositions = new List<Vector3>();
        foreach (FoodData food in foodTable)
        {
            if (food.isSpawned)
            {
                spawnedPositions.Add(food.position);
            }
        }
        return spawnedPositions;
    }

      public void LogSpawnedFoods()
    {
        Debug.Log("Spawned Foods:");
        foreach (FoodData food in foodTable)
        {
            if (food.isSpawned)
            {
                Debug.Log("Registered food ID: " + food.id + ", Position: " + food.position);
            }
        }
    }
}
