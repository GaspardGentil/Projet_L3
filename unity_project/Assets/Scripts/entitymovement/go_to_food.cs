using UnityEngine;
using UnityEngine.AI;

public class FoodHunt : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private EntityProperties entityProperties;
    private HungerSystem hungerSystem;
    private Animal animalScript;
    private bool hasReachedFood = false;

    private void OnTriggerEnter(Collider other)
    {
        // Check if collided object has tag "Food" and if the animal hasn't reached food yet
        if (other.CompareTag("Food") && !hasReachedFood)
        {
            Debug.Log("getting Nav agent");
            navMeshAgent = GetComponent<NavMeshAgent>();

            Debug.Log("getting chicken properties");
            entityProperties = GetComponent<EntityProperties>();
            hungerSystem = GetComponent<HungerSystem>();
            animalScript = GetComponent<Animal>();

            // Disable the Animal script
            animalScript.enabled = false;

            // Reset NavMeshAgent path
            navMeshAgent.ResetPath();

            // Move towards the collided object
            navMeshAgent.SetDestination(other.transform.position);

            hasReachedFood = true;

              if (hasReachedFood)
        {
            Debug.Log("reached food");
            // Disable collider and renderer of the collided object
            Collider collidedCollider = other.GetComponent<Collider>();
            if (collidedCollider != null)
            {
                collidedCollider.enabled = false;
            }

            Renderer collidedRenderer = other.GetComponent<Renderer>();
            if (collidedRenderer != null)
            {
                collidedRenderer.enabled = false;
            }

            // Call IncreaseFertility function of EntityProperties with parameter 1
            entityProperties.IncreaseFertility(1);
            Debug.Log("Fertility increased by 1.");

            // Call IncreaseHunger function from HungerSystem script after increasing fertility
            hungerSystem.IncreaseHunger(20); // Increase hunger by 5
            Debug.Log("Hunger increased by 20.");

            // Re-enable the Animal script
            animalScript.enabled = true;
        }
        // Check if the animal has reached food
      
        }
    }
}
