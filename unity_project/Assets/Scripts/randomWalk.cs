using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class randomWalk : MonoBehaviour
{


    // Start is called before the first frame update

    public float m_Range = 25.0f;
    public NavMeshAgent m_Agent;
    void Start()
    {
        m_Agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_Agent.pathPending || m_Agent.remainingDistance > 0.1f)
        {
            return;
        }

        m_Agent.destination = m_Range * Random.insideUnitCircle;
    }

}
