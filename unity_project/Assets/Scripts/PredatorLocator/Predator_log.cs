using UnityEngine;

public class Predator_log : MonoBehaviour
{
    public PredatorManager Manager;

    void Start()
    {
        // If Manager is not assigned, create a new instance of PredatorManager
        if (Manager == null)
        {
            // Create a new GameObject with PredatorManager script attached
            GameObject managerObject = new GameObject("PredatorManager");
            Manager = managerObject.AddComponent<PredatorManager>();
        }
    }
}