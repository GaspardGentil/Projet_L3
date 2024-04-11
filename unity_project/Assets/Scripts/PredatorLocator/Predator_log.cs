using UnityEngine;

using System.Collections.Generic;

public class Predator_log : MonoBehaviour
{private List<PredatorInfo> predatorList = new List<PredatorInfo>();

    // Function to print the table of predator positions
    public void PrintPredatorTable()
    {
        Debug.Log("Predator Positions:");
        foreach (PredatorInfo predatorInfo in predatorList)
        {
            Debug.Log("Discovered Predator ID: " + predatorInfo.id + ", Position: " + predatorInfo.position);
        }
    }

    // Function to add a value (position) to the table if the ID doesn't already exist
    public void AddToPredatorTable(int id, Vector3 newPosition)
    {
        if (!IsIdInPredatorTable(id))
        {
            PredatorInfo newPredator = new PredatorInfo(id, newPosition);
            predatorList.Add(newPredator);
        }
        else
        {
            Debug.LogWarning("Predator with ID " + id + " already exists in the table.");
        }
    }

    // Function to check if an ID is in the table
    public bool IsIdInPredatorTable(int idToCheck)
    {
        foreach (PredatorInfo predatorInfo in predatorList)
        {
            if (predatorInfo.id == idToCheck)
            {
                return true;
            }
        }
        return false;
    }
}