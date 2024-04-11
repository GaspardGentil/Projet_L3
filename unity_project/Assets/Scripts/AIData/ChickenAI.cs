using UnityEngine;
using System.Collections.Generic;

public class ChickenAI : MonoBehaviour
{
    private FoodLogger foodInformationsScript;
    private Predator_log predatorInformationsScript;

    public List<Vector3> spawnedFoodPositions;
    public List<Vector3> predatorPositions;
    List<HungerSystem> chickenHungerScriptsList;
    List<Animal> chickenRandomMovementScripts;
    void Start()
    {
        foodInformationsScript = FindScript<FoodLogger>("loggerfood"); // class containing info collected about food
        predatorInformationsScript = FindScript<Predator_log>("loggerpredator"); //class containing info collected about predators
        chickenHungerScriptsList = GetChildScriptsByName<HungerSystem>("All_Chicken", "HungerSystem"); // class attached to each chicken containing the current Hunger
        chickenRandomMovementScripts= GetChildScriptsByName<Animal>("All_Chicken", "Animal"); // class attached to each chicken containing the random movement behavior

    }
void Update()
{
    UpdateSpawnedFoodPositions();
    UpdatePredatorPositions();

    

    // Check if the current amount of spawned food is not zero
    if (spawnedFoodPositions.Count > 0)
    {       chickenHungerScriptsList = GetChildScriptsByName<HungerSystem>("All_Chicken", "HungerSystem"); // class attached to each chicken containing the current Hunger
    chickenRandomMovementScripts= GetChildScriptsByName<Animal>("All_Chicken", "Animal"); // class attached to each chicken containing the random movement behavior

        foodInformationsScript.LogSpawnedFoods();
        // Toggle the Animal scripts for a max amount of chickens corresponding to the current spawned food amount
        ToggleAnimalScripts(false);

        // Move those hungry chickens towards the food position that is closest to them
        MoveChickensToFoodIfHungry();


    }
    else{
           ToggleAnimalScripts(true);
    }
}



    void UpdateSpawnedFoodPositions()
    {
        if (foodInformationsScript != null)
        {
            spawnedFoodPositions = foodInformationsScript.GetSpawnedFoodPositions();
        }
    }

    void UpdatePredatorPositions()
    {
        if (predatorInformationsScript != null)
        {
            predatorPositions = predatorInformationsScript.GetPredatorPositions();
        }
    }

    T FindScript<T>(string objectName) where T : MonoBehaviour
    {
        GameObject obj = GameObject.Find(objectName);

        if (obj != null)
        {
            T script = obj.GetComponent<T>();

            if (script != null)
            {
                Debug.Log("Script component of type '" + typeof(T).Name + "' was successfully found on the object named '" + objectName + "'.");
                return script;
            }
            else
            {
                Debug.LogError("Script component of type '" + typeof(T).Name + "' not found on the object named '" + objectName + "'.");
                return null;
            }
        }
        else
        {
            Debug.LogError("Object named '" + objectName + "' not found in the scene.");
            return null;
        }
    }

    // Function to toggle the state of Animal scripts attached to hungry chicken GameObjects
    public void ToggleAnimalScripts(bool enable)
    {

        for (int i = 0; i < chickenHungerScriptsList.Count; i++)
        {
            // Check if the chicken is hungry

            if (chickenHungerScriptsList[i].GetHunger() < 20)
            {
                // Toggle the Animal script state accordingly
                chickenRandomMovementScripts[i].enabled = enable;
               
            }
        }
}




 
    // Function to get scripts of specified type attached to children of a GameObject
    public List<T> GetChildScriptsByName<T>(string parentObjectName, string scriptName) where T : MonoBehaviour
    {
        List<T> scriptsList = new List<T>();

        // Find the parent GameObject with the specified name
        GameObject parentObject = GameObject.Find(parentObjectName);

        if (parentObject != null)
        {
            Debug.Log ("parent object found: "+parentObjectName);
            // Iterate through all children of the parent GameObject
            foreach (Transform child in parentObject.transform)
            {
                // Try to get the script component of specified type attached to the child GameObject
                T script = child.GetComponent(scriptName) as T;

                // If the script is found, add it to the list
                if (script != null)
                {
                    scriptsList.Add(script);
                }
            }
        }
        else
        {
            Debug.LogError("Parent object named '" + parentObjectName + "' not found.");
        }

        return scriptsList;
    }

      void MoveChickensToFoodIfHungry()
    {
        foreach (var hungerScript in chickenHungerScriptsList)
        {
            // Check if hunger is lower than 20
            if (hungerScript.GetHunger() < 30)
            {
                // Find the closest food position
                Vector3 closestFoodPosition = FindClosestFoodPosition(hungerScript.transform.position);

                // Move the chicken towards the food while avoiding predators
                MoveChickenTowardsFood(hungerScript.transform, closestFoodPosition);

            }
        }
    }

    public void EnableAllAnimalScripts()
{
    foreach (var script in chickenRandomMovementScripts)
    {
        if (script != null)
        {
            script.enabled = true;
        }
    }
}


    Vector3 FindClosestFoodPosition(Vector3 currentPosition)
    {
        float minDistance = Mathf.Infinity;
        Vector3 closestFoodPosition = Vector3.zero;

        foreach (var foodPosition in spawnedFoodPositions)
        {
            float distance = Vector3.Distance(currentPosition, foodPosition);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestFoodPosition = foodPosition;
            }
        }

        return closestFoodPosition;
    }

void MoveChickenTowardsFood(Transform chickenTransform, Vector3 foodPosition)
{
    // Get the NavMeshAgent component attached to the chicken
    UnityEngine.AI.NavMeshAgent navMeshAgent = chickenTransform.GetComponent<UnityEngine.AI.NavMeshAgent>();
    Debug.Log("Moving the chicken towards the food");

    // Check if the NavMeshAgent component exists
    if (navMeshAgent != null)
    {
        // Set the destination of the NavMeshAgent to the food position
        navMeshAgent.SetDestination(foodPosition);

        // Specify a small distance threshold
        float smallDistanceThreshold = 1.0f;

        // Check if the chicken is within the small distance threshold to the destination
        if (navMeshAgent.remainingDistance <= smallDistanceThreshold)
        {
            // Get the Animal component attached to the chicken
            Animal randomWalkScript = chickenTransform.GetComponent<Animal>();

            // Check if the Animal component exists
            if (randomWalkScript != null)
            {
                // Enable the random walk script
                randomWalkScript.enabled = true;
                Debug.Log("Random walk script enabled.");
            }
            else
            {
                // If Animal component is missing, log a warning
                Debug.LogWarning("Animal component not found on the chicken.");
            }
        }

        // Check for predators in the path of the chicken
        foreach (Vector3 predatorPosition in predatorPositions)
        {
            if (Vector3.Distance(chickenTransform.position, predatorPosition) < navMeshAgent.radius * 2)
            {
                // If a predator is detected within a certain distance, evade by turning right
                chickenTransform.Rotate(Vector3.up * 45f);
                break; // Exit the loop after evading one predator
            }
        }
    }
    else
    {
        // If NavMeshAgent component is missing, log an error
        Debug.LogError("NavMeshAgent component not found on the chicken.");
    }
}

}
