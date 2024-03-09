using System.Collections;
using UnityEngine;

public class LaChasse : MonoBehaviour
{
    [SerializeField] string tagProie;
    [SerializeField] float predatorSpeed = 10f; // Speed of the predator
    [SerializeField] float chaseDuration = 20f; // Duration of chasing in seconds
    [SerializeField] float aggressiveAlpha = 0.6f; // Alpha value for aggressive color

    GameObject prey; // Reference to the prey GameObject
    bool isMovingTowardsPrey = false; // Flag to indicate if the predator is moving towards the prey
    bool isReturning = false; // Flag to indicate if the predator is returning to its original position
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
        if (isReturning)
        {
            // Rotate the predator to look at the direction it's moving towards
            Vector3 direction = (originalPosition - transform.position).normalized;
            transform.rotation = Quaternion.LookRotation(direction);

            // Move the predator towards its original position
            transform.position = Vector3.MoveTowards(transform.position, originalPosition, predatorSpeed * Time.deltaTime);

            // Check if the predator has reached its original position
            if (transform.position == originalPosition)
            {
                isReturning = false; // Reset the returning flag
                SetMaterialProperties(sphereRenderer, originalColor); // Reset the material color
                Debug.Log("Le prédateur est revenu à sa position d'origine !");
            }
        }
        else if (isMovingTowardsPrey && prey != null)
        {
            // Adjust the alpha component of the original color for aggressive color
            Color aggressiveColor = new Color(originalColor.r, originalColor.g, originalColor.b, aggressiveAlpha);
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

                // Set the returning flag to true
                isReturning = true;
            }
        }
    }

    void StopChasing()
    {
        // Reset prey reference and chasing flag
        prey = null;
        isMovingTowardsPrey = false;
        Debug.Log("Stopped chasing Prey");

        // Set the returning flag to true
        isReturning = true;
        Debug.Log("going back");
    }

    // Function to set the color of the material
    void SetMaterialProperties(Renderer renderer, Color color)
    {
        // Modify the material's color
        Material material = renderer.material;
        material.color = color;
        renderer.material = material;
    }
}
