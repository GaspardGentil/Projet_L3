using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class LaChasse : MonoBehaviour
{
    [SerializeField] string tagPredateur;
    [SerializeField] string tagProie;
    [SerializeField] float detectionRadius = 1f;
    [SerializeField] float predatorSpeed = 2f; // Speed of the predator

    GameObject prey; // Reference to the prey GameObject
    bool isMovingTowardsPrey = false; // Flag to indicate if the predator is moving towards the prey

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
            }
        }
    }

    void Update()
    {
        if (isMovingTowardsPrey && prey != null)
        {
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
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}