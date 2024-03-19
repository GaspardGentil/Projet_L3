using UnityEngine;
using System.Collections.Generic;

public class AllLionsManager : MonoBehaviour
{
    [System.Serializable]
    public class LionData
    {
        public int id;
        public Vector3 position;
        public PredatorInfoMono lionComponent;
    }

    public List<LionData> allLionsInfo = new List<LionData>();
    public GameObject lionPrefab; // Prefab for spawning lions

    private void Start()
    {
        // Get initial positions and IDs of all children lions
        Transform allLions = transform;
        foreach (Transform lion in allLions)
        {
            PredatorInfoMono lionComponent = lion.GetComponent<PredatorInfoMono>();
            if (lionComponent != null)
            {
                int lionID = lionComponent.id;
                Vector3 lionPosition = lionComponent.position;
                LionData lionData = new LionData { id = lionID, position = lionPosition, lionComponent = lionComponent };
                allLionsInfo.Add(lionData);
            }
        }
    }

    private void Update()
    {
        // Check for destroyed lions and respawn them
        for (int i = allLionsInfo.Count - 1; i >= 0; i--)
        {
            if (allLionsInfo[i].lionComponent == null) // Lion component destroyed
            {
                RespawnLion(allLionsInfo[i]);
                allLionsInfo.RemoveAt(i);
            }
        }
    }

    private void RespawnLion(LionData lionData)
    {
        // Delayed respawn after 5 seconds
        float respawnDelay = 5f;
        Invoke("SpawnDelayedLion", respawnDelay); // Invoke a parameterless method

        // Define a parameterless method for invoking
        void SpawnDelayedLion()
        {
            SpawnDelayedLionWithArgument(lionData);
        }
    }

    private void SpawnDelayedLionWithArgument(LionData lionData)
    {
        // Respawn the lion at its original position
        GameObject newLion = Instantiate(lionPrefab, lionData.position, Quaternion.identity);
        PredatorInfoMono newLionComponent = newLion.GetComponent<PredatorInfoMono>();
        if (newLionComponent != null)
        {
            newLionComponent.id = lionData.id;
            newLionComponent.position = lionData.position;
        }
    }
}
