/*
Ce script g�re la journalisation des donn�es sur les aliments dans le jeu.
*/

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

    public List<FoodData> foodTable = new List<FoodData>();

      /*
    Cette fonction ajoute de nouvelles donn�es sur un aliment � la table des aliments.
    @param: position : type: Vector3 : position de l'aliment ajout�
    @returnValue: int : type : l'ID unique attribu� � l'aliment ajout�
    */
$    public int AddFoodData(Vector3 position)
    {
        Debug.Log("Added new food at position: " + position);
        int newId = GenerateUniqueID();
        FoodData newFood = new FoodData();
        newFood.id = newId;
        newFood.position = position;
        newFood.isSpawned = false; 
        foodTable.Add(newFood);
        return newId;
    }
     /*
    Cette fonction g�n�re un ID unique pour un nouvel aliment.
    @returnValue: int : type : un ID unique
    */
    private int GenerateUniqueID()
    {
        int newId;
        do
        {
            newId = UnityEngine.Random.Range(1, int.MaxValue); 
        } while (foodTable.Exists(food => food.id == newId));
        return newId;
    }

      /*
    Cette fonction d�fini l'�tat de spawn d'un aliment dans la table.
    @param: id : type: int : ID de l'aliment
    @param: isSpawned : type: bool : �tat de spawn de l'aliment
    */
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

     /*
    Cette fonction r�initialise l'�tat de spawn de l'aliment apr�s un d�lai.
    @param: id : type: int : ID de l'aliment
    */
    private IEnumerator ResetSpawnedStateAfterDelay(int id)
    {
        yield return new WaitForSeconds(5f);
        SetSpawnedState(id, true);
        Debug.Log("we think the food with ID: " + id + " has respawned");
    }

      /*
    Cette fonction r�cup�re l'�tat de spawn d'un aliment dans la table.
    @param: id : type: int : ID de l'aliment
    @returnValue: bool : type : �tat de spawn de l'aliment
    */
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

        /*
    Cette fonction v�rifie si une position est pr�sente dans la table des aliments.
    @param: position : type: Vector3 : position � v�rifier
    @returnValue: bool : type : vrai si la position est dans la table, sinon faux
    */
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
      /*
    Cette fonction r�cup�re les positions des aliments qui sont spawn�s.
    @returnValue: List<Vector3> : type : liste des positions des aliments spawn�s
    */
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

     /*
    Cette fonction journalise les aliments qui sont spawn�s.
    */
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
