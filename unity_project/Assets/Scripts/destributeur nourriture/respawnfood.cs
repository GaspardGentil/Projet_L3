using UnityEngine;
using System.Collections;

public class Respawn : MonoBehaviour
{
    private Renderer _renderer;
    private BoxCollider _collider;
    private const float TIMER_DURATION = 5f;

    private void Start()
    {
        _renderer = GetComponent<Renderer>();
        _collider = GetComponent<BoxCollider>();

        StartCoroutine(WaitOneSecond());
        _collider.isTrigger = true;
    }

    IEnumerator WaitOneSecond()
    {
        yield return new WaitForSeconds(1f); // Wait for 1 second
        Debug.Log("One second has passed.");
        // Continue with your code here after waiting for 1 second
    }

    public void StartRespawnTimer()
    {
        _renderer.enabled = false;
        _collider.enabled = false;
        Invoke("RespawnObject", TIMER_DURATION);
    }

    private void RespawnObject()
    {
        _renderer.enabled = true;
        _collider.enabled = true;
        _collider.isTrigger = true; // Assuming you want to set isTrigger to true on respawn
    }
}
