using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class randomWalk : MonoBehaviour
{

	public NavMeshAgent Lion;
    public Transform Chicken;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Lion.SetDestination(Chicken.position);
    }
}
