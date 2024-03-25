using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddFoodOnStart : MonoBehaviour
{
    public string resourceName = "Nourriture"; // Nom du prefab à charger depuis le dossier "Resources"
    public static int nourrituresSpawned = 0; // Variable statique pour compter les nourritures spawnées
    public static List<NourritureInfo> spawnedFoodList = new List<NourritureInfo>(); // Liste pour stocker les infos des nourritures spawnées
    
    void Start()
    {
        // Trouve l'objet "Lieu_de_spawn_nourriture" dans la scène
        GameObject lieuDeSpawnNourriture = GameObject.Find("Lieu_de_spawn_nourriture");

        if (lieuDeSpawnNourriture != null)
        {
            // Loop through each child of "Lieu_de_spawn_nourriture"
            foreach (Transform child in lieuDeSpawnNourriture.transform)
            {
                // Obtient les coordonnées de position du child
                Vector3 childPosition = child.position;

                // Charge le prefab depuis le dossier "Resources"
                GameObject foodPrefab = Resources.Load<GameObject>(resourceName);
                
                if (foodPrefab != null)
                {
                    // Instancie l'objet "Nourriture" aux mêmes coordonnées que le child
                    GameObject foodInstance = Instantiate(foodPrefab, childPosition, Quaternion.identity);

                    // Récupère le composant Collider de la nourriture
                    Collider coll = foodInstance.GetComponent<Collider>();
                    if (coll != null)
                    {
                        // Définit isTrigger à true
                        coll.isTrigger = true;
                    }
                    else
                    {
                        Debug.LogWarning("Aucun composant BoxCollider n'a été trouvé sur le prefab '" + resourceName + "'. Assurez-vous qu'un BoxCollider est attaché au prefab.");
                    }

                    // Récupère l'ID de la nourriture
                    int foodID = ++nourrituresSpawned;

                    // Ajoute l'info de la nourriture à la liste des nourritures spawnées
                    spawnedFoodList.Add(new NourritureInfo(foodID, true, childPosition));
                }
                else
                {
                    Debug.LogError("Le prefab '" + resourceName + "' n'a pas pu être chargé depuis le dossier 'Resources'. Assurez-vous que le prefab est correctement placé dans le dossier 'Resources'.");
                }
                LogSpawnedFoodList();   
            }
        }
        else
        {
            Debug.LogError("L'objet 'Lieu_de_spawn_nourriture' n'a pas été trouvé dans la scène. Assurez-vous que l'objet 'Lieu_de_spawn_nourriture' est présent et correctement nommé dans la scène.");
        }
    }

    void LogSpawnedFoodList()
    {
        Debug.Log("Spawned Food List:");
        foreach (var foodInfo in spawnedFoodList)
        {
            Debug.Log("ID: " + foodInfo.id + ", Spawned: " + foodInfo.isSpawned + ", Position: " + foodInfo.position);
        }
    }
}
