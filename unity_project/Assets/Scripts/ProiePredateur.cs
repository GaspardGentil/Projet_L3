using System.Collections;
using UnityEngine;

public class LaChasse : MonoBehaviour
{
    [SerializeField] string tagProie;
    [SerializeField] float predatorSpeed = 10f; // Speed of the predator
    [SerializeField] float chaseDuration = 8f; // Duration of chasing in seconds
    [SerializeField] Color aggressiveColor = new Color(1f, 0f, 0f, 0.5f); // Slightly opaque red color when aggressive
    [SerializeField] Color returnToColor = new Color(1f, 0f, 0f, 0f); // Completely transparent red color when returning to original position

    GameObject prey; // Reference to the prey GameObject
    bool isMovingTowardsPrey = false; // Flag to indicate if the predator is moving towards the prey
    bool returnTo = false; // Flag to indicate if the predator should return to original position
    Vector3 originalPosition; // Original position of the predator

    Renderer sphereRenderer; // Reference to the sphere renderer

    Color originalColor; // Original color of the predator's material

    void Start()
    {
        originalPosition = transform.position; // Store the original position of the predator

        // Find the child GameObject named "Sphere"
        Transform sphereTransform = transform.Find("Sphere");

        // Get the renderer component of the sphere
        if (sphereTransform != null)
        {
            sphereRenderer = sphereTransform.GetComponent<Renderer>();

            // Store the original color of the material
            originalColor = sphereRenderer.material.color;
        }
        else
        {
            Debug.LogError("Sphere not found as a child of the predator!");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(tagProie))
        {
            // Check if there's no prey assigned or if the current prey has been consumed
            if (prey == null)
            {
                prey = other.gameObject;
                isMovingTowardsPrey = true;
                StartCoroutine(ChaseTimer()); // Start the chase timer
            }
        }
    }

    IEnumerator ChaseTimer()
    {
        yield return new WaitForSeconds(chaseDuration);

        // Stop chasing and return to the original position
        StopChasing();
    }

    void Update()
    {
        if (isMovingTowardsPrey && prey != null)
        {
            // Set the predator's color to aggressive color
            SetMaterialProperties(sphereRenderer, aggressiveColor);

            // Rotate the predator to look at the prey's position
            transform.LookAt(prey.transform.position);

            // Move the predator towards the prey at the specified speed
            transform.position = Vector3.MoveTowards(transform.position, prey.transform.position, predatorSpeed * Time.deltaTime);

            // Check if the predator has reached the position of the prey
            if (transform.position == prey.transform.position)
            {
                // Destroy the prey
                Destroy(prey);
                Debug.Log("Le prédateur a tué la proie !");

                // Reset the prey reference and flag
                prey = null;
                isMovingTowardsPrey = false;

                // Set returnTo flag to true to indicate returning to original position
                returnTo = true;
            }
        }

        // If returnTo flag is true, call ReturnToOriginalPosition()
        if (returnTo)
        {
            ReturnToOriginalPosition();
        }
    }

    void StopChasing()
    {
        // Reset prey reference and chasing flag
        prey = null;
        isMovingTowardsPrey = false;
        Debug.Log("Stopped chasing Prey");

        // Reset the predator's color to original color with 0 alpha
        SetMaterialProperties(sphereRenderer, returnToColor);

        // Set returnTo flag to true to indicate returning to original position
        returnTo = true;
        Debug.Log("going back");
    }

    void ReturnToOriginalPosition()
    {
        // Calculate the direction towards the original position
        Vector3 direction = (originalPosition - transform.position).normalized;

        // Rotate the predator to look in the direction of movement
        transform.rotation = Quaternion.LookRotation(direction);

        // Move the predator back to its original position
        transform.position = Vector3.MoveTowards(transform.position, originalPosition, predatorSpeed * Time.deltaTime);

        // Check if the predator is close enough to its original position
        if (Vector3.Distance(transform.position, originalPosition) < 0.1f) // Adjust threshold as needed
        {
            Debug.Log("Le prédateur est revenu à sa position d'origine !");
            returnTo = false; // Reset the returnTo flag
        }
    }

    // Function to set the color of the material
    void SetMaterialProperties(Renderer renderer, Color color)
    {
        // Set the color of the material
        renderer.material.color = color;
    }
}
