using UnityEngine;
using UnityEngine.AI;

public class CollisionChecker : MonoBehaviour
{
    private Proie thisProieScript; 
    private Animal thisAnimalScript; 
    private Proie otherProieScript; 
    private Animal otherAnimalScript;
    private NavMeshAgent thisNavMeshAgent; 
    private NavMeshAgent otherNavMeshAgent; 
    private GameObject predateurObject;
    private LaChasse laChasseScript;

    private bool isMovingTowardsPredateur = false;
    private bool isPredateurDestroyed = false;
private void OnTriggerEnter(Collider other)
{
    if (other.gameObject.CompareTag(gameObject.tag))
    {
        thisProieScript = GetComponent<Proie>();

        thisAnimalScript = GetComponent<Animal>();

        otherProieScript = other.gameObject.GetComponent<Proie>();

        otherAnimalScript = other.gameObject.GetComponent<Animal>();

        thisNavMeshAgent = GetComponent<NavMeshAgent>();

        otherNavMeshAgent = other.gameObject.GetComponent<NavMeshAgent>();

        if (thisProieScript != null && thisProieScript.isFleeing)
        {
            Debug.Log("Current object is fleeing.");

            predateurObject = thisProieScript.predateur;

            if (otherProieScript == null)
            {
                Debug.Log("Friend Proie destroyed. Stopping fleeing and enabling all scripts.");
                StopFleeing();
                return; 
            }

            if (predateurObject != null)
            {
               laChasseScript = predateurObject.GetComponent<LaChasse>();

                if (laChasseScript != null)
                {
                    Debug.Log("LaChasse script found on predateur attached to Proie script.");
                }
                else
                {
                    Debug.Log("LaChasse script not found on predateur attached to Proie script.");
                }
            }
            else
            {
                Debug.Log("Predateur GameObject not found attached to Proie script.");
                StopFleeing();
                return; 
            }

            thisProieScript.isFleeing = false;
            if (otherProieScript != null)
            {
                otherProieScript.isFleeing = false;
            }

            DisableAllScripts();
            ResetNavMeshAgents();

            MoveTowardsPredateur();

            isMovingTowardsPredateur = true;
        }
    }
}

  /*
    Cette méthode arrête la fuite et réactive tous les scripts.
    */
private void StopFleeing()
{
    Debug.Log("Stopping fleeing and enabling all scripts.");
    isMovingTowardsPredateur = false;
    EnableAllScripts();
}

 /*
    Cette méthode désactive tous les scripts.
    */
 private void DisableAllScripts()
    {
        if (thisProieScript != null)
        {
            thisProieScript.enabled = false;
        }

        if (thisAnimalScript != null)
        {
            thisAnimalScript.enabled = false;
        }

        if (otherProieScript != null)
        {
            otherProieScript.enabled = false;
        }

        if (otherAnimalScript != null)
        {
            otherAnimalScript.enabled = false;
        }

        if (laChasseScript != null)
        {
            laChasseScript.enabled = false;
        }
    }
     /*
    Cette méthode active tous les scripts.
    */
    private void ResetNavMeshAgents()
    {
        if (thisNavMeshAgent != null)
        {
            thisNavMeshAgent.ResetPath();
            Debug.Log("NavMeshAgent reset for current object.");
        }

        if (otherNavMeshAgent != null)
        {
            otherNavMeshAgent.ResetPath();
            Debug.Log("NavMeshAgent reset for other collided object.");
        }
    }
    private void EnableAllScripts()
{
    if (thisProieScript != null)
    {
        thisProieScript.enabled = true;
    }

    if (thisAnimalScript != null)
    {
        thisAnimalScript.enabled = true;
    }


    if (otherProieScript != null)
    {
        otherProieScript.enabled = true;
    }


    if (otherAnimalScript != null)
    {
        otherAnimalScript.enabled = true;
    }

    if (laChasseScript != null)
    {
        laChasseScript.enabled = true;
    }
}
  /*
    Cette méthode déplace les entités vers le prédateur.
    */
    private void MoveTowardsPredateur()
{
    if (thisNavMeshAgent != null && otherNavMeshAgent != null && predateurObject != null)
    {
        Vector3 midpoint = (transform.position + otherNavMeshAgent.transform.position + predateurObject.transform.position) / 3f;

        thisNavMeshAgent.SetDestination(midpoint);

        otherNavMeshAgent.SetDestination(midpoint);
    }
}


    private void Update()
    {
        if (isMovingTowardsPredateur && !isPredateurDestroyed)
        {
            if (thisNavMeshAgent != null && otherNavMeshAgent != null &&
                !thisNavMeshAgent.pathPending && !otherNavMeshAgent.pathPending &&
                thisNavMeshAgent.remainingDistance <= thisNavMeshAgent.stoppingDistance &&
                otherNavMeshAgent.remainingDistance <= otherNavMeshAgent.stoppingDistance &&
                !thisNavMeshAgent.hasPath && !otherNavMeshAgent.hasPath)
            {
                Destroy(predateurObject);

                Debug.Log("Predateur destroyed.");

                isPredateurDestroyed = true;
                EnableAllScripts();
            }
        }
    }
}
