using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Profiling.Memory.Experimental;
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
    [Header("Wander")] //distance max 
    public float wanderDistance = 50f;
    public float walkSpeed = 5f;
    public float maxWalkTime = 6f;

    [Header("Idle")] // pause de l'animal
    public float idleTime = 1f;

    protected NavMeshAgent navMeshAgent;
    protected AnimalState currentState  = AnimalState.Idle;

    //Liste des informations de la nourriture
    protected List<NourritureInfo> foodInfos = new List<NourritureInfo>();

    private void Start()
    {
        InitialiseAnimal();
    }

    protected virtual void InitialiseAnimal()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
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
        Vector3 randomDirection = Random.insideUnitSphere * distance; // point random
        randomDirection += origin;
        NavMeshHit navMeshHit;

        if(NavMesh.SamplePosition(randomDirection,out navMeshHit, distance, NavMesh.AllAreas))
        {
            return navMeshHit.position;
        }
        else
        {
            return GetRandomNavMeshPosition(origin, distance);
        }
    }


    protected virtual void HandleIdleState()
    {
        StartCoroutine(WaitToMove());
    }

    private IEnumerator WaitToMove()
    {
        float waitTime = Random.Range(idleTime / 2, idleTime * 2);
        yield return new WaitForSeconds(waitTime);

        Vector3 randomDestination;
        if (foodInfos.Count > 0)
        {
            int randomIndex = Random.Range(0, foodInfos.Count);
            randomDestination = foodInfos[randomIndex].position;
            //foodInfos.RemoveAt(randomIndex);

        }else
        {
            randomDestination = GetRandomNavMeshPosition(transform.position, wanderDistance);
        }

        navMeshAgent.SetDestination(randomDestination);
        SetState(AnimalState.Moving);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Food"))
        {
            Vector3 foodPosition = other.transform.position;
            int foodId = other.GetComponent<NourritureInfo>().id;
            bool foodSpawned = other.GetComponent<NourritureInfo>().isSpawned;
            AddFoodInfo(new NourritureInfo(foodId,foodSpawned,foodPosition));

            //Destroy(other.gameObject);
        }
    }
    public void AddFoodInfo(NourritureInfo info)
    {
        foodInfos.Add(info);
    }

    protected virtual void HandleMovingState()
    {
        StartCoroutine(WaitToReachDestination());
    }

    private IEnumerator WaitToReachDestination()
    {
        float startTime = Time.time;

        while(navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance)
        {
            if(Time.time - startTime >= maxWalkTime)
            {
                navMeshAgent.ResetPath();
                SetState(AnimalState.Idle);
                yield break;
            }

            yield return null;
        }

        SetState(AnimalState.Idle);
    }

    protected void SetState(AnimalState newState)
    {
        if(currentState == newState) return;

        currentState = newState;
        OnStateChange(newState);
    }

    protected virtual void OnStateChange(AnimalState newState)
    {
        UpdateState();
    }
}
