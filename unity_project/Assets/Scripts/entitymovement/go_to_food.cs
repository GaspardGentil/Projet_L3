using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class go_to_food : MonoBehaviour
{
    private Animal randomWalkScript; // Référence au script Animal
    private NavMeshAgent navMeshAgent;

    protected List<NourritureInfo> foodInfos = new List<NourritureInfo>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Food"))
        {
            if (randomWalkScript != null)
            {
                randomWalkScript.enabled = false;
            }

            // Affichage pour le débogage
            Debug.Log("La poule a pris de la nourriture");

            Vector3 foodPosition = other.transform.position;
            int foodId = other.GetComponent<NourritureInfo>().id;
            bool foodSpawned = other.GetComponent<NourritureInfo>().isSpawned;
            foodSpawned = false;
            AddFoodInfo(new NourritureInfo(foodId, foodSpawned, foodPosition));

            Destroy(other.gameObject);
            Debug.Log("La nourriture est détruite");

            if (navMeshAgent != null)
            {
                navMeshAgent.ResetPath();
            }
        }
    }

    public void AddFoodInfo(NourritureInfo info)
    {
        foodInfos.Add(info);

        // Affichage pour le débogage
        Debug.Log("La nourriture est ajoutée à la liste");
    }
}
