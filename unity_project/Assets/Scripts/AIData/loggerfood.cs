/*
Ce script gère la journalisation des données sur les aliments dans le jeu.
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
    Cette fonction ajoute de nouvelles données sur un aliment à la table des aliments.
    @param: position : type: Vector3 : position de l'aliment ajouté
    @returnValue: int : type : l'ID unique attribué à l'aliment ajouté
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
    Cette fonction génère un ID unique pour un nouvel aliment.
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
    Cette fonction défini l'état de spawn d'un aliment dans la table.
    @param: id : type: int : ID de l'aliment
    @param: isSpawned : type: bool : état de spawn de l'aliment
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
    Cette fonction réinitialise l'état de spawn de l'aliment après un délai.
    @param: id : type: int : ID de l'aliment
    */
    private IEnumerator ResetSpawnedStateAfterDelay(int id)
    {
        yield return new WaitForSeconds(5f);
        SetSpawnedState(id, true);
        Debug.Log("we think the food with ID: " + id + " has respawned");
    }

      /*
    Cette fonction récupère l'état de spawn d'un aliment dans la table.
    @param: id : type: int : ID de l'aliment
    @returnValue: bool : type : état de spawn de l'aliment
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
    Cette fonction vérifie si une position est présente dans la table des aliments.
    @param: position : type: Vector3 : position à vérifier
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
    Cette fonction récupère les positions des aliments qui sont spawnés.
    @returnValue: List<Vector3> : type : liste des positions des aliments spawnés
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
    Cette fonction journalise les aliments qui sont spawnés.
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
