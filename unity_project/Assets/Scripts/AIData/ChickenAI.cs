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

    public void ToggleAnimalScripts(bool enable)
{
    foreach (var script in chickenRandomMovementScripts)
    {
        script.enabled = enable;
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
}
