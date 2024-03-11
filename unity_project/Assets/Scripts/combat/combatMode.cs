using UnityEngine;

public class CollisionChecker : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Check if the collided object has the same tag
        if (other.gameObject.CompareTag(gameObject.tag))
        {
            // Get the Proie script component attached to the collided object
            Proie proieScript = other.gameObject.GetComponent<Proie>();

            // Check if the current object's isFleeing variable is true
            if (proieScript != null && proieScript.isFleeing)
            {
                // Perform actions when the conditions are met
                Debug.Log("Collision with an object of the same tag while fleeing!");
                // Add your desired actions here
            }
        }
    }
}
