using UnityEngine;

public class respawn : MonoBehaviour
{
    private Renderer _renderer;
    private BoxCollider _collider;
    private bool _isRendererEnabled = true;
    private float _timer = 0f;
    private const float TIMER_DURATION = 10f;

    private void Start()
    {
        _renderer = GetComponent<Renderer>();
        _collider = GetComponent<BoxCollider>();
    }

    private void Update()
    {
        // Check if renderer is enabled
        if (_renderer.enabled != _isRendererEnabled)
        {
            if (_renderer.enabled)
            {
                Debug.Log("Renderer Enabled");
            }
            else
            {
                Debug.Log("Renderer Disabled. Starting Timer...");
                _timer = TIMER_DURATION;
            }
            _isRendererEnabled = _renderer.enabled;
        }

        // If renderer is disabled, start timer
        if (!_isRendererEnabled)
        {
            _timer -= Time.deltaTime;
            if (_timer <= 0)
            {
                Debug.Log("Timer Finished. Enabling Renderer and Collider...");
                _renderer.enabled = true;
                _collider.enabled = true;
                _isRendererEnabled = true;
            }
        }
    }
}
