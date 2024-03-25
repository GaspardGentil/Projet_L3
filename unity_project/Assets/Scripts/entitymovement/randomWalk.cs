using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public enum AnimalState
{
    Idle,
    Moving,
}

[RequireComponent(typeof(NavMeshAgent))]
public class Animal : MonoBehaviour
{
    [Header("Wander")]
    public float wanderDistance = 50f;
    public float walkSpeed = 5f;
    public float maxWalkTime = 6f;

    [Header("Idle")]
    public float idleTime = 5f;

    protected NavMeshAgent navMeshAgent;
    protected AnimalState currentState = AnimalState.Idle;
    protected bool isObjectActive = true;

    private Coroutine currentCoroutine;

    private void Start()
    {
        InitialiseAnimal();
    }

    protected virtual void InitialiseAnimal()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        if (navMeshAgent == null)
        {
            Debug.LogError("NavMeshAgent component not found.");
            return;
        }
        navMeshAgent.speed = walkSpeed;

        currentState = AnimalState.Idle;
        UpdateState();
    }

    protected virtual void UpdateState()
    {
        switch (currentState)
        {
            case AnimalState.Idle:
                HandleIdleState();
                break;
            case AnimalState.Moving:
                HandleMovingState();
                break;
        }
    }

    protected Vector3 GetRandomNavMeshPosition(Vector3 origin, float distance)
    {
        Vector3 randomDirection = Random.insideUnitSphere * distance;
        randomDirection += origin;
        NavMeshHit navMeshHit;

        if (NavMesh.SamplePosition(randomDirection, out navMeshHit, distance, NavMesh.AllAreas))
        {
            return navMeshHit.position;
        }
        else
        {
            Debug.LogWarning("Unable to find valid position on NavMesh.");
            return origin;
        }
    }

    protected virtual void HandleIdleState()
    {
        currentCoroutine = StartCoroutine(WaitToMove());
    }

    private IEnumerator WaitToMove()
    {
        float waitTime = Random.Range(idleTime / 2, idleTime * 2);
        yield return new WaitForSeconds(waitTime);

        if (navMeshAgent == null)
        {
            Debug.LogError("NavMeshAgent component not found.");
            yield break;
        }

        Vector3 randomDestination = GetRandomNavMeshPosition(transform.position, wanderDistance);

        if (isObjectActive)
        {
            navMeshAgent.SetDestination(randomDestination);
            SetState(AnimalState.Moving);
        }
    }

    protected virtual void HandleMovingState()
    {
        currentCoroutine = StartCoroutine(WaitToReachDestination());
    }

    private IEnumerator WaitToReachDestination()
    {
        float startTime = Time.time;

        if (navMeshAgent == null)
        {
            Debug.LogError("NavMeshAgent component not found.");
            yield break;
        }

        while (navMeshAgent != null && navMeshAgent.isActiveAndEnabled && navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance)
        {
            if (!isObjectActive) // Check if the object is still active before continuing the loop
                break;

            if (Time.time - startTime >= maxWalkTime)
            {
                navMeshAgent.ResetPath();
                SetState(AnimalState.Idle);
                break;
            }

            yield return null;
        }

        SetState(AnimalState.Idle);
    }

    protected void SetState(AnimalState newState)
    {
        if (currentState == newState) return;

        currentState = newState;
        OnStateChange(newState);
    }

    protected virtual void OnStateChange(AnimalState newState)
    {
        UpdateState();
    }

    private void OnDestroy()
    {
        isObjectActive = false;
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }
    }
}
