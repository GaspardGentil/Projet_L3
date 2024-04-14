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
        foodInformationsScript = FindScript<FoodLogger>("loggerfood"); 
        predatorInformationsScript = FindScript<Predator_log>("loggerpredator"); 
        chickenHungerScriptsList = GetChildScriptsByName<HungerSystem>("All_Chicken", "HungerSystem");
        chickenRandomMovementScripts= GetChildScriptsByName<Animal>("All_Chicken", "Animal");


    }
    void Update()
    {
        UpdateSpawnedFoodPositions();
        UpdatePredatorPositions();

    

        if (spawnedFoodPositions.Count > 0)
        {    

            foodInformationsScript.LogSpawnedFoods();
            ToggleAnimalScripts(false);

            MoveChickensToFoodIfHungry();


        }
        else{
               ToggleAnimalScripts(true);
        }
    }

    /*
    Cette fonction ajoute un script de faim à la liste des scripts de faim et un script d'animal à la liste des scripts de mouvement.
    @param: hungerScript : type: HungerSystem : script de faim à ajouter à la liste
    @param: movementScript : type: Animal : script d'animal à ajouter à la liste
    */
    public void AddHungerAndMovementScripts(HungerSystem hungerScript, Animal movementScript)
    {
        // Ajouter le script de faim à la liste des scripts de faim
        if (hungerScript != null)
        {
            if (!chickenHungerScriptsList.Contains(hungerScript))
            {
                chickenHungerScriptsList.Add(hungerScript);
                Debug.Log("Le script de faim est ajouté à la liste.");
            }
            else
            {
                Debug.LogWarning("Le script de faim est déjà dans la liste.");
            }
        }
        else
        {
            Debug.LogWarning("Le script de faim est null.");
        }

        // Ajouter le script d'animal à la liste des scripts de mouvement
        if (movementScript != null)
        {
            if (!chickenRandomMovementScripts.Contains(movementScript))
            {
                chickenRandomMovementScripts.Add(movementScript);
                 Debug.Log("Le script de mouvement est ajouté à la liste.");
            }
            else
            {
                Debug.LogWarning("Le script de mouvement est déjà dans la liste.");
            }
        }
        else
        {
            Debug.LogWarning("Le script de mouvement est null.");
        }
    }


    /*
    Cette fonction met à jour les positions des aliments spawnés.
    @param: foodInformationsScript : type: FoodLogger : script contenant des informations sur les aliments
    */

    void UpdateSpawnedFoodPositions()
    {
        if (foodInformationsScript != null)
        {
            spawnedFoodPositions = foodInformationsScript.GetSpawnedFoodPositions();
        }
    }

     /*
    Cette fonction met à jour les positions des prédateurs.
    @param: predatorInformationsScript : type: Predator_log : script contenant des informations sur les prédateurs
    */
    void UpdatePredatorPositions()
    {
        if (predatorInformationsScript != null)
        {
            predatorPositions = predatorInformationsScript.GetPredatorPositions();
        }
    }
     /*
    Cette fonction trouve un script de type spécifié attaché à un GameObject.
    @param: objectName : type: string : nom de l'objet à chercher
    @returnValue: T : type : script trouvé, null s'il n'est pas trouvé
    */

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
    for (int i = 0; i < chickenRandomMovementScripts.Count; i++)
    {
        // Check if the Animal component is not null and is not destroyed
        if (chickenRandomMovementScripts[i] != null)
        {
            if (chickenHungerScriptsList[i].GetHunger() < 20)
            {
                // Enable or disable the Animal component based on the parameter
                chickenRandomMovementScripts[i].enabled = enable;
            }
        }
        else
        {
            // If the Animal component is null or destroyed, remove it from the list
            chickenRandomMovementScripts.RemoveAt(i);
            // Adjust the index to account for the removal
            i--;
        }
    }
}



 
     /*
    Cette fonction récupère les scripts d'un type spécifié attachés aux enfants d'un GameObject.
    @param: parentObjectName : type: string : nom de l'objet parent à chercher
    @param: scriptName : type: string : nom du script à rechercher
    @returnValue: List<T> : type : liste des scripts trouvés
    */
    public List<T> GetChildScriptsByName<T>(string parentObjectName, string scriptName) where T : MonoBehaviour
    {
        List<T> scriptsList = new List<T>();

        GameObject parentObject = GameObject.Find(parentObjectName);

        if (parentObject != null)
        {
            Debug.Log ("parent object found: "+parentObjectName);
            foreach (Transform child in parentObject.transform)
            {
                T script = child.GetComponent(scriptName) as T;

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
        if (hungerScript == null)
        {
            // Skip null objects
            continue;
        }

        EntityProperties currentChickenProperties = hungerScript.GetComponent<EntityProperties>();
        if (currentChickenProperties == null)
        {
            // Skip objects without EntityProperties component
            continue;
        }

        if (hungerScript.GetHunger() > 30 && currentChickenProperties.GetFertility() > 0)
        {
            // Check if the current chicken is fertile
            // Find a fertile chicken of the opposite sex
            Animal nearestFertileChicken = FindNearestFertileChicken(hungerScript.transform.position, currentChickenProperties.GetSex());
            if (nearestFertileChicken != null)
            {
                MoveChickenTowardsFertileChicken(hungerScript.transform, nearestFertileChicken.transform.position);
            }
        }
        else if (hungerScript.GetHunger() < 30)
        {
            Vector3 closestFoodPosition = FindClosestFoodPosition(hungerScript.transform.position);
            MoveChickenTowardsFood(hungerScript.transform, closestFoodPosition);
        }
    }
}



Animal FindNearestFertileChicken(Vector3 currentPosition, EntityProperties.Sex currentChickenSex)
{
    Animal nearestFertileChicken = null;
    float minDistance = Mathf.Infinity;

    foreach (var mouvementscript in chickenRandomMovementScripts)
    {       
        if (mouvementscript != null)
        {
         EntityProperties otherChickenProperties = mouvementscript.transform.GetComponent<EntityProperties>();
        if (otherChickenProperties != null && otherChickenProperties.GetFertility() > 0 && otherChickenProperties.GetSex() != currentChickenSex)
        {
            float distance = Vector3.Distance(currentPosition, mouvementscript.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestFertileChicken = mouvementscript;
            }
        }
        }
       
    }

    return nearestFertileChicken;
}

void MoveChickenTowardsFertileChicken(Transform chickenTransform, Vector3 chickenPosition)
{
    UnityEngine.AI.NavMeshAgent navMeshAgent = chickenTransform.GetComponent<UnityEngine.AI.NavMeshAgent>();
    Debug.Log("Moving the chicken towards the other chicken");

    if (navMeshAgent != null)
    {
        navMeshAgent.SetDestination(chickenPosition);
        // Handle remaining distance and predator detection as required
    }
    else
    {
        Debug.LogError("NavMeshAgent component not found on the chicken.");
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

     /*
    Cette fonction déplace les poulets vers la nourriture s'ils ont faim.
    @param: chickenTransform : type: Transform : transform du poulet
    @param: foodPosition : type: Vector3 : position de la nourriture
    */
void MoveChickenTowardsFood(Transform chickenTransform, Vector3 foodPosition)
{
    UnityEngine.AI.NavMeshAgent navMeshAgent = chickenTransform.GetComponent<UnityEngine.AI.NavMeshAgent>();
    Debug.Log("Moving the chicken towards the food");

    if (navMeshAgent != null)
    {
        navMeshAgent.SetDestination(foodPosition);

        float smallDistanceThreshold = 1.0f;

        if (navMeshAgent.remainingDistance <= smallDistanceThreshold)
        {
            Animal randomWalkScript = chickenTransform.GetComponent<Animal>();

            if (randomWalkScript != null)
            {
                randomWalkScript.enabled = true;
                Debug.Log("Random walk script enabled.");
            }
            else
            {
                Debug.LogWarning("Animal component not found on the chicken.");
            }
        }

        foreach (Vector3 predatorPosition in predatorPositions)
        {
            if (Vector3.Distance(chickenTransform.position, predatorPosition) < navMeshAgent.radius * 2)
            {
                chickenTransform.Rotate(Vector3.up * 45f);
                break; 
            }
        }
    }
    else
    {
        Debug.LogError("NavMeshAgent component not found on the chicken.");
    }
}

}
