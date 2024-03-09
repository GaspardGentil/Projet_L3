using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LaChasse : MonoBehaviour
{
    [SerializeField] string tagPredateur;
    [SerializeField] string tagProie;
    [SerializeField] float detectionRadius = 1f;
    [SerializeField] float predatorSpeed = 10f; // Speed of the predator
    [SerializeField] float chaseDuration = 8f; // Duration of chasing in seconds

    GameObject prey; // Reference to the prey GameObject
    bool isMovingTowardsPrey = false; // Flag to indicate if the predator is moving towards the prey
    Vector3 originalPosition; // Original position of the predator

    void Start()
    {
        originalPosition = transform.position; // Store the original position of the predator
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(tagPredateur))
        {
            Debug.Log("Les prédateurs ne se mangent pas entre eux !");
        }
    }

    void OnTriggerStay(Collider other)
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

                // Return to the original position
                ReturnToOriginalPosition();
            }
        }
    }

    void StopChasing()
    {
        // Reset prey reference and chasing flag
        prey = null;
        isMovingTowardsPrey = false;

        // Return to the original position
        ReturnToOriginalPosition();
    }

    void ReturnToOriginalPosition()
    {
        // Move the predator back to its original position
        transform.position = Vector3.MoveTowards(transform.position, originalPosition, predatorSpeed * Time.deltaTime);

        // Check if the predator has reached its original position
        if (transform.position == originalPosition)
        {
            Debug.Log("Le prédateur est revenu à sa position d'origine !");
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
