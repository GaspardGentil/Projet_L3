using UnityEngine;
using System.Collections;

public class RespawnEnfant : MonoBehaviour
{
    public GameObject enfantPrefab; // Prefab of the child to respawn
    private Transform[] enfantPositions; // Array of positions where the children are present

    public float verificationInterval = 5f; // Time between each check for missing child

    private void Start()
    {
        // Retrieve current positions of the children
        int childCount = transform.childCount;
        enfantPositions = new Transform[childCount];
        for (int i = 0; i < childCount; i++)
        {
            enfantPositions[i] = transform.GetChild(i);
        }

        // Start a coroutine to check for missing children every few seconds
        StartCoroutine(CheckMissingEnfant());
    }

    IEnumerator CheckMissingEnfant()
    {
        while (true)
        {
            // Iterate through all child positions
            foreach (Transform position in enfantPositions)
            {
                // If no child is present at this position
                if (position.childCount == 0)
                {
                    // Instantiate a new child at this position and make it a child of the position
                    GameObject nouvelEnfant = Instantiate(enfantPrefab, position.position, Quaternion.identity);
                    nouvelEnfant.transform.parent = position;
                }
            }
            // Wait for a certain time before checking again
            yield return new WaitForSeconds(verificationInterval);
        }
    }
}
