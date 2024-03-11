using UnityEngine;

public class TriggerWithPredateur : MonoBehaviour
{
    public string predateurTag = "Lion"; // Tag of the Predateur object
    private float collisionCooldown = 10f; // Time to wait before logging another collision
    private float lastCollisionTime = -10f; // Time of the last collision (initialized to allow immediate logging)

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(predateurTag))
        {
            // Check if enough time has passed since the last collision
            if (Time.time - lastCollisionTime >= collisionCooldown)
            {
                Debug.Log("Collided with a predator object!");

                // Find the loggerpredator object in the scene
                GameObject loggerPredator = GameObject.Find("loggerpredator");

                if (loggerPredator != null)
                {
                    // Get the Predator_log script component from loggerpredator
                    Predator_log predatorLogScript = loggerPredator.GetComponent<Predator_log>();

                    if (predatorLogScript != null)
                    {
                        // Call AddToPredatorTable method of the Manager script and pass the position of the collision
                        if (!predatorLogScript.Manager.IsPositionInPredatorTable(transform.position))
                        {
                            predatorLogScript.Manager.AddToPredatorTable(transform.position);
                            Debug.Log("Added predator location: " + transform.position);
                        }
                    }
                    else
                    {
                        Debug.LogError("Predator_log script component not found on loggerpredator object.");
                    }
                }
                else
                {
                    Debug.LogError("loggerpredator object not found in the scene.");
                }

                // Update the last collision time
                lastCollisionTime = Time.time;
            }
        }
    }
}
