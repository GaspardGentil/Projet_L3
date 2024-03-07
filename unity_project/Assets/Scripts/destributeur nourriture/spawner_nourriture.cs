using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AddFoodOnStart : MonoBehaviour
{
    public string resourceName = "Nourriture"; // Nom du prefab à charger depuis le dossier "Resources"
    public static int nourrituresSpawned = 0; // Variable statique pour compter les nourritures spawnées
    public static List<NourritureInfo> spawnedFoodList = new List<NourritureInfo>(); // Liste pour stocker les infos des nourritures spawnées

    private int maxNourritures; // Variable pour stocker le nombre maximum de nourritures

    void Start()
    {
        // Crée un objet vide "food_despenser"
        GameObject foodDespenser = new GameObject("food_despenser");

        // Ajoute l'objet "food_despenser" à la scène
        Instantiate(foodDespenser);

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

                    // Place l'instance de nourriture comme enfant de l'objet "food_despenser"
                    foodInstance.transform.parent = foodDespenser.transform;

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

        // Stocke le nombre maximum de nourritures
        maxNourritures = nourrituresSpawned;

        // Démarre la coroutine pour vérifier le respawn des nourritures
        StartCoroutine(CheckRespawn());
    }

    IEnumerator CheckRespawn()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f); // Attend une seconde avant de vérifier à nouveau

            // Vérifie si le nombre de nourritures spawnées a diminué
            if (nourrituresSpawned < maxNourritures)
            {
                // Démarre le timer pour le respawn après 20 secondes
                StartCoroutine(RespawnTimer());
            }
        }
    }

    IEnumerator RespawnTimer()
    {
        yield return new WaitForSeconds(20f); // Attend 20 secondes avant de respawn

        // Recherche une nourriture non spawnée dans la liste des nourritures
        NourritureInfo missingFood = spawnedFoodList.Find(food => !food.isSpawned);

        if (missingFood != null)
        {
            // Charge le prefab depuis le dossier "Resources"
            GameObject foodPrefab = Resources.Load<GameObject>(resourceName);

            if (foodPrefab != null)
            {
                // Instancie l'objet "Nourriture" à la position enregistrée
                GameObject foodInstance = Instantiate(foodPrefab, missingFood.position, Quaternion.identity);

                // Incrémente le nombre de nourritures spawnées
                nourrituresSpawned++;

                // Marque la nourriture comme spawnée
                missingFood.isSpawned = true;

                // Log the ID table with each ID and spawn boolean
                LogSpawnedFoodList();
            }
            else
            {
                Debug.LogError("Le prefab '" + resourceName + "' n'a pas pu être chargé depuis le dossier 'Resources'. Assurez-vous que le prefab est correctement placé dans le dossier 'Resources'.");
            }
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

 