using UnityEngine;
using UnityEngine.AI;

public class Proie : MonoBehaviour
{
    [SerializeField] float movementSpeed = 1f; // Speed of the prey
    [SerializeField] float fleeSpeedMultiplier = 0.5f; // Multiplier for speed when fleeing from predator
     public GameObject predateur=null; // Reference to the predator GameObject
    [SerializeField] float rotationSpeed = 5f; // Speed at which the prey rotates
    [SerializeField] float fleeDistance = 10f; // Distance at which the prey stops fleeing
    [SerializeField] string tagPredateur;
    [SerializeField] public bool isFleeing = false; // Flag to indicate if the prey is fleeing from the predator

    private Animal randomWalkScript; // Reference to the RandomWalk script
    private NavMeshAgent navMeshAgent; // Reference to the NavMeshAgent component

    void Start()
    {
        // Get the Animal and NavMeshAgent components attached to the prey
        randomWalkScript = GetComponent<Animal>();
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        // Check if the prey is fleeing from the predator
        if (isFleeing && predateur!=null)
        {
            // Calculate the direction away from the predator
            Vector3 fleeDirection = transform.position - predateur.transform.position;
            fleeDirection.y = 0f; // Ensure movement is only in the horizontal plane

            // Move the prey in the opposite direction of the predator at a reduced speed
            navMeshAgent.SetDestination(transform.position + fleeDirection.normalized * fleeDistance);

            // Rotate the prey to face the direction it's moving in
            Quaternion targetRotation = Quaternion.LookRotation(fleeDirection.normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the collided object has the specified tag
        if (other.CompareTag(tagPredateur))
        {
            // Assign the collided object as the predator
            predateur = other.gameObject;

            // Disable the RandomWalk script attached to the prey
            if (randomWalkScript != null)
            {
                randomWalkScript.enabled = false;
            }

            // Reset the NavMeshAgent's path
            if (navMeshAgent != null)
            {
                navMeshAgent.ResetPath();
            }

            // Start fleeing from the predator
            isFleeing = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Check if the prey exits the collider of the predator
        if (other.CompareTag(tagPredateur) && other.gameObject == predateur)
        {
            // Re-enable the RandomWalk script attached to the prey
            if (randomWalkScript != null)
            {
                randomWalkScript.enabled = true;
            }

            // Stop fleeing from the predator
            isFleeing = false;
        }
    }
}
