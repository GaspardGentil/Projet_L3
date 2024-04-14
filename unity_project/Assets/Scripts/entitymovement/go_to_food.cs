using UnityEngine;
using UnityEngine.AI;

public class FoodHunt : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private EntityProperties entityProperties;
    private HungerSystem hungerSystem;
    private Animal animalScript;
    private Respawn respawnfoodscript;
    private FoodLogger foodLogger; // Reference to FoodLogger script
    private Vector3 foodPosition; // 'vector3' should be 'Vector3'

    private void Start()
    {
        // Initialize references to required components
        navMeshAgent = GetComponent<NavMeshAgent>();
        entityProperties = GetComponent<EntityProperties>();
        hungerSystem = GetComponent<HungerSystem>();
        animalScript = GetComponent<Animal>();

        // Attempt to find the loggerfood GameObject
        GameObject loggerFoodObject = GameObject.Find("loggerfood");

        // Check if the loggerfood GameObject is found
        if (loggerFoodObject != null)
        {
            // Attempt to get the FoodLogger script attached to the loggerfood GameObject
            foodLogger = loggerFoodObject.GetComponent<FoodLogger>();

            // Check if the FoodLogger script is found
            if (foodLogger != null)
            {
                // FoodLogger script is successfully obtained
                Debug.Log("FoodLogger script found!");
            }
            else
            {
                // FoodLogger script is not found on loggerfood GameObject
                Debug.LogWarning("FoodLogger script is not attached to the loggerfood GameObject!");
            }
        }
        else
        {
            // loggerfood GameObject is not found in the scene
            Debug.LogWarning("loggerfood GameObject not found in the scene!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if collided object has tag "Food"
        if (other.CompareTag("Food"))
        {
            foodPosition = other.transform.position;

            // Cache respawn script
            respawnfoodscript = other.GetComponent<Respawn>();

            // Disable the Food Collider
            other.enabled = false;

            // Enable the Animal script
            animalScript.enabled = true;

            // Stop going to food
            navMeshAgent.ResetPath();

            // Increase fertility and hunger
            entityProperties.IncreaseFertility(1);
            Debug.Log("Fertility increased by 1.");

            hungerSystem.IncreaseHunger(20); // Increase hunger by 20
            Debug.Log("Hunger increased by 20.");

            // Initiate respawn timer for the food
            if (respawnfoodscript != null)
                respawnfoodscript.StartRespawnTimer();

            Debug.Log("Respawning the Food");

            if (foodLogger != null && !foodLogger.IsPositionInTable(foodPosition)) // Corrected 'foodLogger.IsPositionInTable'
            {
                // position is not in table so we add it
                int id = foodLogger.AddFoodData(foodPosition); // Corrected 'foodLogger.AddFoodData'
                foodLogger.SetSpawnedState(id, false);
            }

            int idfood=foodLogger.GetFoodID(foodPosition);

            if (idfood!=-1)
            {
                  foodLogger.SetSpawnedState(idfood, false);
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if collided object has tag "Food"
        if (other.CompareTag("Food"))
        {
            // Re-enable the Food Collider
            other.enabled = true;
        }
    }
}
