using UnityEngine;
using System.Collections.Generic;

public class PredatorManager : MonoBehaviour
{
    private List<Vector3> predatorPositions = new List<Vector3>();

    // Function to print the table of predator positions
    public void PrintPredatorTable()
    {
        Debug.Log("Predator Positions:");
        foreach (Vector3 position in predatorPositions)
        {
            Debug.Log(position);
        }
    }

    // Function to add a value (position) to the table
    public void AddToPredatorTable(Vector3 newPosition)
    {
        predatorPositions.Add(newPosition);
    }

    // Function to check if a value (position) is in the table
    public bool IsPositionInPredatorTable(Vector3 positionToCheck)
    {
        return predatorPositions.Contains(positionToCheck);
    }
}
