using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaChasse : MonoBehaviour
{
    [SerializeField] string tagPredateur;
    [SerializeField] string tagProie;
    [SerializeField] float detectionRadius = 1f;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(tagPredateur))
        {
            Debug.Log("Les pr�dateurs ne se mangent pas entre eux !");
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(tagProie))
        {
            Destroy(other.gameObject);
            Debug.Log("Le pr�dateur a tu� la proie !");
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
