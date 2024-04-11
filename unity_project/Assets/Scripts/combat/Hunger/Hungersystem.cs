using UnityEngine;

public class HungerSystem : MonoBehaviour
{
    public int maxHunger = 60;
    public int currentHunger;

    private float hungerDepletionRate = 1f; // 1 hunger per second

    private void Start()
    {
        currentHunger = maxHunger;
        InvokeRepeating(nameof(DepleteHunger), 1f, 1f); // Start depleting hunger every second
    }

    private void DepleteHunger()
    {
        if (currentHunger > 0)
        {
            currentHunger -= 1;
            if (currentHunger <= 0)
            {
                LogBeforeDestroy();
                DestroyEntity();
            }
        }
    }

    private void DestroyEntity()
    {
        // Implement destruction logic here
        Debug.Log("Entity destroyed due to hunger!");
        Destroy(this.gameObject); // Destroy only the specific object this script is attached to
    }

    private void LogBeforeDestroy()
    {
        Debug.Log("Entity's hunger reached zero. Destroying entity...");
    }

    // Public function to increase hunger by a certain amount
    public void IncreaseHunger(int amount)
    {
        currentHunger += amount;
        currentHunger = Mathf.Clamp(currentHunger, 0, maxHunger);
    }

    public int GetHunger()
    {
        return currentHunger;
    }
}
