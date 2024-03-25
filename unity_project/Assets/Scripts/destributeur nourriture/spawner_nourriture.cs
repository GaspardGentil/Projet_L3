using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddFoodOnStart : MonoBehaviour
{
    public string resourceName = "Nourriture"; // Nom du prefab � charger depuis le dossier "Resources"
    public static int nourrituresSpawned = 0; // Variable statique pour compter les nourritures spawn�es
    public static List<NourritureInfo> spawnedFoodList = new List<NourritureInfo>(); // Liste pour stocker les infos des nourritures spawn�es
    
    void Start()
    {
        // Trouve l'objet "Lieu_de_spawn_nourriture" dans la sc�ne
        GameObject lieuDeSpawnNourriture = GameObject.Find("Lieu_de_spawn_nourriture");

        if (lieuDeSpawnNourriture != null)
        {
            // Loop through each child of "Lieu_de_spawn_nourriture"
            foreach (Transform child in lieuDeSpawnNourriture.transform)
            {
                // Obtient les coordonn�es de position du child
                Vector3 childPosition = child.position;

                // Charge le prefab depuis le dossier "Resources"
                GameObject foodPrefab = Resources.Load<GameObject>(resourceName);
                
                if (foodPrefab != null)
                {
                    // Instancie l'objet "Nourriture" aux m�mes coordonn�es que le child
                    GameObject foodInstance = Instantiate(foodPrefab, childPosition, Quaternion.identity);

                    // R�cup�re le composant Collider de la nourriture
                    Collider coll = foodInstance.GetComponent<Collider>();
                    if (coll != null)
                    {
                        // D�finit isTrigger � true
                        coll.isTrigger = true;
                    }
                    else
                    {
                        Debug.LogWarning("Aucun composant BoxCollider n'a �t� trouv� sur le prefab '" + resourceName + "'. Assurez-vous qu'un BoxCollider est attach� au prefab.");
                    }

                    // R�cup�re l'ID de la nourriture
                    int foodID = ++nourrituresSpawned;

                    // Ajoute l'info de la nourriture � la liste des nourritures spawn�es
                    spawnedFoodList.Add(new NourritureInfo(foodID, true, childPosition));
                }
                else
                {
                    Debug.LogError("Le prefab '" + resourceName + "' n'a pas pu �tre charg� depuis le dossier 'Resources'. Assurez-vous que le prefab est correctement plac� dans le dossier 'Resources'.");
                }
                LogSpawnedFoodList();   
            }
        }
        else
        {
            Debug.LogError("L'objet 'Lieu_de_spawn_nourriture' n'a pas �t� trouv� dans la sc�ne. Assurez-vous que l'objet 'Lieu_de_spawn_nourriture' est pr�sent et correctement nomm� dans la sc�ne.");
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
