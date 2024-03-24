using UnityEngine;
using UnityEngine.AI;

public class CollisionChecker : MonoBehaviour
{
    private Proie thisProieScript; // Proie script attached to the current object
    private Animal thisAnimalScript; // Animal script attached to the current object
    private Proie otherProieScript; // Proie script attached to the other collided object
    private Animal otherAnimalScript; // Animal script attached to the other collided object
    private NavMeshAgent thisNavMeshAgent; // NavMeshAgent attached to the current object
    private NavMeshAgent otherNavMeshAgent; // NavMeshAgent attached to the other collided object
    private GameObject predateurObject; // GameObject predateur attached to Proie script of the current object
    private LaChasse laChasseScript; // LaChasse script attached to Predateur

    private bool isMovingTowardsPredateur = false; // Flag to indicate if Proies are moving towards the predateur
    private bool isPredateurDestroyed = false; // Flag to indicate if the predateur is destroyed
private void OnTriggerEnter(Collider other)
{
    // Check if the other object has the same tag
    if (other.gameObject.CompareTag(gameObject.tag))
    {
        // Get the Proie script component attached to the current object
        thisProieScript = GetComponent<Proie>();

        // Get the Animal script component attached to the current object
        thisAnimalScript = GetComponent<Animal>();

        // Get the Proie script component attached to the other collided object
        otherProieScript = other.gameObject.GetComponent<Proie>();

        // Get the Animal script component attached to the other collided object
        otherAnimalScript = other.gameObject.GetComponent<Animal>();

        // Get the NavMeshAgent attached to the current object
        thisNavMeshAgent = GetComponent<NavMeshAgent>();

        // Get the NavMeshAgent attached to the other collided object
        otherNavMeshAgent = other.gameObject.GetComponent<NavMeshAgent>();

        // Check if the current object's isFleeing variable is true
        if (thisProieScript != null && thisProieScript.isFleeing)
        {
            Debug.Log("Current object is fleeing.");

            // Get the GameObject predateur attached to Proie script of the current object
            predateurObject = thisProieScript.predateur;

            if (otherProieScript == null)
            {
                Debug.Log("Friend Proie destroyed. Stopping fleeing and enabling all scripts.");
                // Stop the fleeing process and enable all scripts
                StopFleeing();
                return; // Exit the method to prevent further execution
            }

            if (predateurObject != null)
            {
                // Get the LaChasse script attached to Predateur
                laChasseScript = predateurObject.GetComponent<LaChasse>();

                if (laChasseScript != null)
                {
                    Debug.Log("LaChasse script found on predateur attached to Proie script.");
                    // Now you can use laChasseScript for further actions
                }
                else
                {
                    Debug.Log("LaChasse script not found on predateur attached to Proie script.");
                }
            }
            else
            {
                Debug.Log("Predateur GameObject not found attached to Proie script.");
                // Stop the fleeing process and enable all scripts
                StopFleeing();
                return; // Exit the method to prevent further execution
            }

            // Set isFleeing to false for both objects
            thisProieScript.isFleeing = false;
            if (otherProieScript != null)
            {
                otherProieScript.isFleeing = false;
            }

            // Disable other scripts attached to both objects
            DisableAllScripts();
            ResetNavMeshAgents();

            // Move both Proies towards the predateur
            MoveTowardsPredateur();

            // Set flag to indicate that Proies are moving towards predateur
            isMovingTowardsPredateur = true;
        }
    }
}

// Method to stop fleeing and enable all scripts
private void StopFleeing()
{
    Debug.Log("Stopping fleeing and enabling all scripts.");
    isMovingTowardsPredateur = false;
    EnableAllScripts();
}

 private void DisableAllScripts()
    {
        // Disable the Proie script attached to the current object
        if (thisProieScript != null)
        {
            thisProieScript.enabled = false;
        }

        // Disable the Animal script attached to the current object
        if (thisAnimalScript != null)
        {
            thisAnimalScript.enabled = false;
        }

        // Disable the Proie script attached to the other collided object
        if (otherProieScript != null)
        {
            otherProieScript.enabled = false;
        }

        // Disable the Animal script attached to the other collided object
        if (otherAnimalScript != null)
        {
            otherAnimalScript.enabled = false;
        }

        // Disable the LaChasse script attached to Predateur
        if (laChasseScript != null)
        {
            laChasseScript.enabled = false;
        }
    }

    private void ResetNavMeshAgents()
    {
        // Reset the NavMeshAgent for the current object
        if (thisNavMeshAgent != null)
        {
            thisNavMeshAgent.ResetPath();
            Debug.Log("NavMeshAgent reset for current object.");
        }

        // Reset the NavMeshAgent for the other collided object
        if (otherNavMeshAgent != null)
        {
            otherNavMeshAgent.ResetPath();
            Debug.Log("NavMeshAgent reset for other collided object.");
        }
    }
    private void EnableAllScripts()
{
    // Enable the Proie script attached to the current object
    if (thisProieScript != null)
    {
        thisProieScript.enabled = true;
    }

    // Enable the Animal script attached to the current object
    if (thisAnimalScript != null)
    {
        thisAnimalScript.enabled = true;
    }

    // Enable the Proie script attached to the other collided object
    if (otherProieScript != null)
    {
        otherProieScript.enabled = true;
    }

    // Enable the Animal script attached to the other collided object
    if (otherAnimalScript != null)
    {
        otherAnimalScript.enabled = true;
    }

    // Enable the LaChasse script attached to Predateur
    if (laChasseScript != null)
    {
        laChasseScript.enabled = true;
    }
}

    private void MoveTowardsPredateur()
{
    if (thisNavMeshAgent != null && otherNavMeshAgent != null && predateurObject != null)
    {
        // Calculate the midpoint between the current positions of the Proies and the predateur's position
        Vector3 midpoint = (transform.position + otherNavMeshAgent.transform.position + predateurObject.transform.position) / 3f;

        // Set destination of NavMeshAgent to the calculated midpoint for the current object
        thisNavMeshAgent.SetDestination(midpoint);

        // Set destination of NavMeshAgent to the calculated midpoint for the other collided object
        otherNavMeshAgent.SetDestination(midpoint);
    }
}


    private void Update()
    {
        // Check if Proies are moving towards predateur
        if (isMovingTowardsPredateur && !isPredateurDestroyed)
        {
            // Check if Proies have reached predateur
            if (thisNavMeshAgent != null && otherNavMeshAgent != null &&
                !thisNavMeshAgent.pathPending && !otherNavMeshAgent.pathPending &&
                thisNavMeshAgent.remainingDistance <= thisNavMeshAgent.stoppingDistance &&
                otherNavMeshAgent.remainingDistance <= otherNavMeshAgent.stoppingDistance &&
                !thisNavMeshAgent.hasPath && !otherNavMeshAgent.hasPath)
            {
                // Destroy the predateur
                Destroy(predateurObject);

                Debug.Log("Predateur destroyed.");

                // Set flag to indicate that the predateur is destroyed
                isPredateurDestroyed = true;
                EnableAllScripts();
            }
        }
    }
}
