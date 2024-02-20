using UnityEngine;

public class AddFoodOnStart : MonoBehaviour
{
    public string resourceName = "Nourriture"; // Nom du prefab � charger depuis le dossier "Resources"

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

                // Log the child's position for debugging
                Debug.Log("Child Position: " + childPosition);

                // Charge le prefab depuis le dossier "Resources"
                GameObject foodPrefab = Resources.Load<GameObject>(resourceName);

                if (foodPrefab != null)
                {
                    // Instancie l'objet "Nourriture" aux m�mes coordonn�es que le child
                    GameObject foodInstance = Instantiate(foodPrefab, childPosition, Quaternion.identity);
                }
                else
                {
                    Debug.LogError("Le prefab '" + resourceName + "' n'a pas pu �tre charg� depuis le dossier 'Resources'. Assurez-vous que le prefab est correctement plac� dans le dossier 'Resources'.");
                }
            }
        }
        else
        {
            Debug.LogError("L'objet 'Lieu_de_spawn_nourriture' n'a pas �t� trouv� dans la sc�ne. Assurez-vous que l'objet 'Lieu_de_spawn_nourriture' est pr�sent et correctement nomm� dans la sc�ne.");
        }
    }
}
