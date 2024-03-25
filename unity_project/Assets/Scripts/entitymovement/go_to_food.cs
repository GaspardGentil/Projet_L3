using UnityEngine;
using UnityEngine.AI;

public class FoodHunt : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private EntityProperties entityProperties;
    private HungerSystem hungerSystem;
    private Animal animalScript;
    private Respawn respawnfoodscript;

    private void Start()
    {
        // Initialize references to required components
        navMeshAgent = GetComponent<NavMeshAgent>();
        entityProperties = GetComponent<EntityProperties>();
        hungerSystem = GetComponent<HungerSystem>();
        animalScript = GetComponent<Animal>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if collided object has tag "Food"
        if (other.CompareTag("Food"))
        {
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
